using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.CompanyQueries;
using JurayKV.Application.Queries.IdentityKvAdQueries;
using JurayKV.Application.Queries.KvPointQueries;
using JurayKV.Application.Queries.TransactionQueries;
using JurayKV.Application.Queries.WalletQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.UserManagerQueries
{
    public sealed class GetFullUserManagerByIdQuery : IRequest<FullUserManagerDetailsDto>
    {
        public GetFullUserManagerByIdQuery(Guid id)
        {
            Id = id.ThrowIfEmpty(nameof(id));
        }

        public Guid Id { get; }

        // Handler
        private class GetFullUserManagerByIdQueryHandler : IRequestHandler<GetFullUserManagerByIdQuery, FullUserManagerDetailsDto>
        {
            private readonly IUserManagerCacheRepository _userManager;
            private readonly IQueryRepository _repository;
            private readonly IMediator _mediator;
            private readonly UserManager<ApplicationUser> _user;



            public GetFullUserManagerByIdQueryHandler(IUserManagerCacheRepository userManager, IQueryRepository repository, IMediator mediator, UserManager<ApplicationUser> user)
            {
                _userManager = userManager;
                _repository = repository;
                _mediator = mediator;
                _user = user;
            }

            public async Task<FullUserManagerDetailsDto> Handle(GetFullUserManagerByIdQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));
                FullUserManagerDetailsDto data = new FullUserManagerDetailsDto();
                var user = await _userManager.GetByIdAsync(request.Id);
                var reff = await _userManager.GetReferralInfoByPhoneAsync(user.RefferedBy);
                var outcome = new UserManagerDetailsDto
                {
                    Id = user.Id,
                    Fullname = user.Fullname,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    CreationUTC = user.CreationUTC, // Replace with the actual property in ApplicationUser
                    AccountStatus = user.AccountStatus,
                    LastLoggedInAtUtc = user.LastLoggedInAtUtc,
                    Surname = user.Surname,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    IsCompany = user.IsCompany,
                    RefferedBy = reff.Fullname,
                    PhoneOfRefferedBy = user.RefferedBy,
                    Tier = user.Tier,
                    RequestDateTie2Upgraded = user.RequestDateTie2Upgraded,
                    About = user.About,
                    AlternativePhone = user.AlternativePhone,
                    Address = user.Address,
                    State = user.State,
                    LGA = user.LGA,
                    Occupation = user.Occupation,
                    FbHandle = user.FbHandle,
                    InstagramHandle = user.InstagramHandle,
                    TwitterHandle = user.TwitterHandle,
                    TiktokHandle = user.TiktokHandle,
                    IDCardKey = user.IDCardKey,
                    IDCardUrl = user.IDCardUrl,
                    PassportUrl = user.PassportUrl,
                    PassportKey = user.PassportKey,
                    AccountName = user.AccountName,
                    AccountNumber = user.AccountNumber,
                    BankName = user.BankName,
                    BVN = user.BVN,
                    DateUpgraded = user.DateUpgraded,
                    ResponseOnCsaRequest = user.ResponseOnCsaRequest,
                    CsaRequest = user.CsaRequest,
                    IsCSARole = user.IsCSARole,
                    Tie2Request = user.Tie2Request,
                    NinNumber = user.NinNumber
                };
                data.UserManagerDetailsDto = outcome;
                if (outcome.IsCompany)
                {
                    GetCompanyByUserIdQuery cmCommand = new GetCompanyByUserIdQuery(outcome.Id);
                    data.CompanyDetailsDto = await _mediator.Send(cmCommand);
                }
                try
                {
                    GetWalletUserByIdQuery wtCommand = new GetWalletUserByIdQuery(outcome.Id);
                    data.WalletDetailsDto = await _mediator.Send(wtCommand);
                }
                catch (Exception c) { }
                try
                {
                    GetUserManagerByReferralListQuery refCommand = new GetUserManagerByReferralListQuery(outcome.PhoneNumber);
                    var refdata = await _mediator.Send(refCommand);
                    data.TotalReferrals = refdata.Count();
                    data.LastTenReferrals = refdata.OrderByDescending(x => x.CreationUTC).ToList();
                }
                catch (Exception e) { }

                try
                {
                    GetKvPointListByUserIdQuery PntCommand = new GetKvPointListByUserIdQuery(outcome.Id);

                    var Points = await _mediator.Send(PntCommand);
                    data.PointCount = Points.Count();
                    data.PointSum = Points.Sum(x => x.Point);
                    data.LastTenPoints = Points.OrderByDescending(x => x.CreatedAtUtc).Take(10).ToList();
                }
                catch (Exception e) { }
                try
                {
                    GetTransactionListByUserCountQuery txtCommand = new GetTransactionListByUserCountQuery(outcome.Id, 50);
                    var Transaction = await _mediator.Send(txtCommand);
                    var creditTotal = Transaction
     .Where(x => x.TransactionType == Domain.Primitives.Enum.TransactionTypeEnum.Credit);
                    data.TransactionCreditCount = creditTotal.Count();
                    //credit
                    var creditTransactions = creditTotal.GroupBy(x => x.UniqueReference)
     .Select(group => new TransactionDescription
     {
         Description = group.Key,
         ListTransactions = group.ToList()
     })
     .ToList();

                    var debitTotal = Transaction
     .Where(x => x.TransactionType == Domain.Primitives.Enum.TransactionTypeEnum.Debit);
                    data.TransactionDebitCount = debitTotal.Count();
                    var debitTransactions = debitTotal
                        .GroupBy(x => x.UniqueReference)
                        .Select(group => new TransactionDescription
                        {
                            Description = group.Key,
                            ListTransactions = group.Take(30).ToList()
                        })
                        .ToList();


                    data.GroupByDescriptionCredit = creditTransactions.Take(20).ToList();
                    
                    data.GroupByDescriptionDebit = debitTransactions.Take(20).ToList();
                    
                }
                catch { }

                try
                {
                    GetIdentityKvAdByUserIdListQuery adccommand = new GetIdentityKvAdByUserIdListQuery(outcome.Id);

                    var advertpost = await _mediator.Send(adccommand);

                    data.PostingCount = advertpost.Count();
                    data.UploadCount = advertpost.Where(x=> !String.IsNullOrEmpty(x.VideoUrl)).Count(); 
                }
                catch { }
                return data;
            }
        }
    }

}
