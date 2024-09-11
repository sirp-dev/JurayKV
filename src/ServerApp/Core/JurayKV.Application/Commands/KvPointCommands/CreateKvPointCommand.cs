using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Commands.TransactionCommands;
using JurayKV.Application.Commands.WalletCommands;
using JurayKV.Application.Queries.SettingQueries;
using JurayKV.Application.Queries.TransactionQueries;
using JurayKV.Application.Queries.UserManagerQueries;
using JurayKV.Application.Queries.WalletQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.IdentityKvAdAggregate;
using JurayKV.Domain.Aggregates.KvPointAggregate;
using JurayKV.Domain.Aggregates.TransactionAggregate;
using JurayKV.Domain.Aggregates.WalletAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Commands.KvPointCommands;

public sealed class CreateKvPointCommand : IRequest<Guid>
{
    public CreateKvPointCommand(Guid userId, Guid identityKvAdId, EntityStatus status, int point, string pointHash)
    {

        UserId = userId;
        IdentityKvAdId = identityKvAdId;
        Status = status;
        Point = point;
        PointHash = pointHash;
    }

    public Guid UserId { get; set; }
    public Guid IdentityKvAdId { get; set; }
    public EntityStatus Status { get; set; }

    public int Point { get; set; }
    public string PointHash { get; set; }
}

internal class CreateKvPointCommandHandler : IRequestHandler<CreateKvPointCommand, Guid>
{
    private readonly IKvPointRepository _kvPointRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IIdentityKvAdRepository _identityKvAdRepository;
    private readonly IKvPointCacheHandler _kvPointCacheHandler;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWalletCacheHandler _walletCacheHandler;
    private readonly IIdentityKvAdCacheHandler _identityKvAdCacheHandler;
    private readonly IRepository _repository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionCacheHandler _transactionCacheHandler;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ISettingCacheRepository _settingCacheRepository;
    private readonly IMediator _mediator;

    public CreateKvPointCommandHandler(
            IKvPointRepository kvPointRepository,
            IKvPointCacheHandler kvPointCacheHandler,
            IWalletRepository walletRepository,
            IHttpContextAccessor httpContextAccessor,
            IWalletCacheHandler walletCacheHandler,
            IIdentityKvAdRepository identityKvAdRepository,
            IIdentityKvAdCacheHandler identityKvAdCacheHandler,
            IRepository repository,
            ITransactionRepository transactionRepository,
            ITransactionCacheHandler transactionCacheHandler,
            UserManager<ApplicationUser> userManager,
            ISettingCacheRepository settingCacheRepository,
            IMediator mediator)
    {
        _kvPointRepository = kvPointRepository;
        _kvPointCacheHandler = kvPointCacheHandler;
        _walletRepository = walletRepository;
        _httpContextAccessor = httpContextAccessor;
        _walletCacheHandler = walletCacheHandler;
        _identityKvAdRepository = identityKvAdRepository;
        _identityKvAdCacheHandler = identityKvAdCacheHandler;
        _repository = repository;
        _transactionRepository = transactionRepository;
        _transactionCacheHandler = transactionCacheHandler;
        _userManager = userManager;
        _settingCacheRepository = settingCacheRepository;
        _mediator = mediator;
    }

    public async Task<Guid> Handle(CreateKvPointCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));

        var check = await _kvPointRepository.GetByIdentityIdByUserAsync(request.IdentityKvAdId, request.UserId);
        if (check == null)
        {
            IDbContextTransaction dbContextTransaction = await _repository
             .BeginTransactionAsync(IsolationLevel.Unspecified, cancellationToken);
            try
            {
                //if null, create
                KvPoint create = new KvPoint(Guid.NewGuid());
                create.UserId = request.UserId;
                create.IdentityKvAdId = request.IdentityKvAdId;
                create.Status = request.Status;
                create.Point = request.Point;
                create.PointHash = request.PointHash;
                create.LastModifiedAtUtc = DateTime.UtcNow;
                // Persist to the database

                Guid resultId = await _kvPointRepository.InsertAsync(create);
                if (resultId != Guid.Empty)
                {
                    //update wallet
                    var getwallet = await _walletRepository.GetByUserIdAsync(request.UserId);
                    getwallet.Amount += Convert.ToDecimal(request.Point);
                    getwallet.LastUpdateAtUtc = DateTime.UtcNow.AddHours(1);
                    var loguserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                    getwallet.Log =  "<li> Wallet Update from User Advert id " + request.IdentityKvAdId + " ::Amount: " + request.Point + " ::Balance: " + getwallet.Amount + " :: Date: " + getwallet.LastUpdateAtUtc + ":: Loggedin User: " + loguserId + "</li>"+ getwallet.Log;

                    await _walletRepository.UpdateAsync(getwallet);

                    await _walletCacheHandler.RemoveListAsync();

                    await _walletCacheHandler.RemoveDetailsByIdAsync(getwallet.Id);
                    await _walletCacheHandler.RemoveDetailsByUserIdAsync(getwallet.UserId);
                    await _walletCacheHandler.RemoveGetAsync(getwallet.Id);

                    //update creadited
                    var getidentitykvads = await _identityKvAdRepository.GetByIdAsync(request.IdentityKvAdId);
                    if (getidentitykvads != null)
                    {
                        getidentitykvads.AdsStatus = AdsStatus.Credited;
                        await _identityKvAdRepository.UpdateAsync(getidentitykvads);
                    }


                    // Remove the cache
                    await _identityKvAdCacheHandler.RemoveListAsync();
                    await _identityKvAdCacheHandler.RemoveListActiveTodayAsync();
                    await _identityKvAdCacheHandler.RemoveGetByUserIdAsync(create.UserId);
                    await _identityKvAdCacheHandler.RemoveGetActiveByUserIdAsync(create.UserId);
                    await _identityKvAdCacheHandler.RemoveDetailsByIdAsync(create.Id);
                    await _identityKvAdCacheHandler.RemoveGetAsync(create.Id);

                    //get the company by identitykvid
                    var identityKvAdsInfo = await _identityKvAdRepository.GetByIdAsync(request.IdentityKvAdId);


                    //debit wallet from company
                    var companywallet = await _walletRepository.GetByUserIdAsync(identityKvAdsInfo.KvAd.Company.UserId);
                    companywallet.Amount -= (Convert.ToDecimal(request.Point) * identityKvAdsInfo.KvAd.Company.AmountPerPoint);
                    companywallet.LastUpdateAtUtc = DateTime.UtcNow.AddHours(1);
                    companywallet.Log = "<li> Wallet Update from User Advert id " + request.IdentityKvAdId + " ::Amount: " + request.Point + " ::Balance: " + companywallet.Amount + " :: Date: " + companywallet.LastUpdateAtUtc + ":: Loggedin User: " + loguserId + "</li>" + companywallet.Log;

                    await _walletRepository.UpdateAsync(companywallet);

                    //create debit transaction for company
                    Transaction Companytransaction = new Transaction(Guid.NewGuid());
                    Companytransaction.WalletId = companywallet.Id;
                    Companytransaction.TransactionType = TransactionTypeEnum.Debit;
                    Companytransaction.TransactionReference = Guid.NewGuid().ToString();
                    Companytransaction.Description = "Advert Debit";
                    Companytransaction.Status = EntityStatus.Successful;
                    Companytransaction.UserId = companywallet.UserId;
                    Companytransaction.Amount = request.Point * identityKvAdsInfo.KvAd.Company.AmountPerPoint;
                    //Companytransaction.Note = "ADs";
                    //Companytransaction.Note = "ADs";

                    // Persist to the database
                    await _transactionRepository.InsertAsync(Companytransaction);
                    //
                    //
                    //check if its firsttime
                    var checkSuccess = await _kvPointRepository.CheckFirstPoint(request.UserId);
                    if (checkSuccess == true)
                    {
                        var userUpdate = await _userManager.FindByIdAsync(request.UserId.ToString());
                        if (userUpdate != null)
                        {
                            userUpdate.SuccessPoint = true;
                            var datacheck = await _userManager.UpdateAsync(userUpdate);
                            if (datacheck.Succeeded)
                            {
                                var settings = await _settingCacheRepository.GetSettingAsync();
                                if (settings.DisableReferralBonus == false)
                                {
                                    //if the referral is not null, credit the referral
                                    //get user by id
                                    GetUserManagerByPhoneQuery getuserbyphonecommand = new GetUserManagerByPhoneQuery(userUpdate.RefferedByPhoneNumber);
                                    var UserWHoReferredTheseAccount = await _mediator.Send(getuserbyphonecommand);
                                    if (UserWHoReferredTheseAccount != null)
                                    {
                                        //get settings
                                        GetSettingDefaultQuery settingscommand = new GetSettingDefaultQuery();
                                        var settingData = await _mediator.Send(settingscommand);


                                        //if transaction is debit.
                                        GetWalletUserByIdQuery walletcommand = new GetWalletUserByIdQuery(UserWHoReferredTheseAccount.Id);
                                        var getwalletReff = await _mediator.Send(walletcommand);

                                        //create the transaction

                                        CreateTransactionCommand transactioncreatecommand = new CreateTransactionCommand(getwalletReff.Id, getwalletReff.UserId, "Referral Bonus", null,
                                            settingData.DefaultReferralAmmount,
                                        TransactionTypeEnum.Credit, EntityStatus.Successful, Guid.NewGuid().ToString(), "Referral Bonus", Guid.NewGuid().ToString() + "-REFERRAL " + userUpdate.IdNumber);
                                        var transaction = await _mediator.Send(transactioncreatecommand);
                                        //GET transaction information to update wallet
                                        //get the transaction by id
                                        GetTransactionByIdQuery gettranCommand = new GetTransactionByIdQuery(transaction);
                                        var thetransaction = await _mediator.Send(gettranCommand);
                                        //update walet
                                        getwalletReff.Amount = getwalletReff.Amount + settingData.DefaultReferralAmmount;

                                        var loguserIdReff = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                                        getwalletReff.Log = "<li>Referral Bonus- Wallet Update from " + thetransaction.Description + " " + thetransaction.Id + " ::Amount: " + thetransaction.Amount + " ::Balance: " + getwalletReff.Amount + " :: Date: " + getwalletReff.LastUpdateAtUtc + ":: Loggedin User: " + loguserIdReff + "</li>" +getwalletReff.Log;
                                        //getwallet = null;
                                        UpdateWalletCommand updatewalletcommand = new UpdateWalletCommand(getwalletReff.UserId, "Validate Transaction", getwalletReff.Log, getwalletReff.Amount);
                                        await _mediator.Send(updatewalletcommand);
                                    }
                                }
                            }
                        }
                    }


                    // Remove the cache
                    await _transactionCacheHandler.RemoveListAsync();
                    await _transactionCacheHandler.RemoveGetAsync(Companytransaction.Id);
                    await _transactionCacheHandler.RemoveDetailsByIdAsync(Companytransaction.Id);
                    await _transactionCacheHandler.RemoveList10ByUserAsync(Companytransaction.UserId);

                }


                // Remove the cache
                await _kvPointCacheHandler.RemoveListAsync();
                await _kvPointCacheHandler.RemoveDetailsByIdAsync(create.Id);
                await _kvPointCacheHandler.RemoveListBy10ByUserAsync(create.UserId);
                await _kvPointCacheHandler.RemoveGetAsync(create.Id);

                return create.Id;

                await dbContextTransaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
        else
        {
            //check.Point = request.Point;
            check.LastModifiedAtUtc = DateTime.UtcNow;
            await _kvPointRepository.UpdateAsync(check);

            // Remove the cache
            await _kvPointCacheHandler.RemoveListAsync();
            await _kvPointCacheHandler.RemoveDetailsByIdAsync(check.Id);
            await _kvPointCacheHandler.RemoveListBy10ByUserAsync(check.UserId);
            await _kvPointCacheHandler.RemoveListByUserAsync(check.UserId);
            await _kvPointCacheHandler.RemoveGetAsync(check.Id);

            return check.Id;
        }
        //check



        //update







    }
}