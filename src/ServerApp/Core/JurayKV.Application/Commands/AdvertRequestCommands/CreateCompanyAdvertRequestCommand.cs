using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Infrastructures;
using JurayKV.Application.Queries.CompanyQueries;
using JurayKV.Domain.Aggregates.AdvertRequestAggregate;
using JurayKV.Domain.Aggregates.SettingAggregate;
using JurayKV.Domain.Aggregates.TransactionAggregate;
using JurayKV.Domain.ValueObjects;
using JurayKV.Infrastructure.Flutterwave.Dtos;
using JurayKV.Infrastructure.Flutterwave.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Commands.AdvertRequestCommands
{
 

public sealed class CreateCompanyAdvertRequestCommand : IRequest<AdvertResponse>
    {
        public CreateCompanyAdvertRequestCommand(AdvertRequest advertRequest, IFormFile file, Guid userId)
        {
            AdvertRequest = advertRequest;
            File = file;
            UserId = userId;
        }

        public AdvertRequest AdvertRequest { get; set; }
        public IFormFile? File { get; set; }
        public Guid UserId { get; set; }
    }

    internal class CreateCompanyAdvertRequestCommandHandler : IRequestHandler<CreateCompanyAdvertRequestCommand, AdvertResponse>
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IAdvertRequestRepository _advertRequestRepository;
        private readonly IAdvertRequestCacheHandler _advertRequestCacheHandler;
        private readonly IStorageService _storage;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionCacheHandler _transactionCacheHandler;
        private readonly ICompanyCacheRepository _companyCacheRepository;
        private readonly IWalletCacheRepository _walletCacheRepository;
        private readonly IRepository _repository;

        private readonly IFlutterTransactionService _repositoryService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateCompanyAdvertRequestCommandHandler(
                IAdvertRequestRepository advertRequestRepository,
                IAdvertRequestCacheHandler advertRequestCacheHandler,
                IStorageService storage,
                ITransactionRepository transactionRepository,
                ITransactionCacheHandler transactionCacheHandler,
                ICompanyCacheRepository companyCacheRepository,
                IWalletCacheRepository walletCacheRepository,
                IFlutterTransactionService repositoryService,
                IHttpContextAccessor httpContextAccessor,
                IRepository repository,
                ISettingRepository settingRepository)
        {
            _advertRequestRepository = advertRequestRepository;
            _advertRequestCacheHandler = advertRequestCacheHandler;
            _storage = storage;
            _transactionRepository = transactionRepository;
            _transactionCacheHandler = transactionCacheHandler;
            _companyCacheRepository = companyCacheRepository;
            _walletCacheRepository = walletCacheRepository;
            _repositoryService = repositoryService;
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
            _settingRepository = settingRepository;
        }

        public async Task<AdvertResponse> Handle(CreateCompanyAdvertRequestCommand request, CancellationToken cancellationToken)
        {
            AdvertResponse responseData = new AdvertResponse();
            _ = request.ThrowIfNull(nameof(request));
            IDbContextTransaction dbContextTransaction = await _repository
              .BeginTransactionAsync(IsolationLevel.Unspecified, cancellationToken);

            try
            {

                var companyDetailsDto = await _companyCacheRepository.GetByUserIdAsync(request.UserId);

            var walletdto = await _walletCacheRepository.GetByUserIdAsync(request.UserId);

            var TransactionReference = Guid.NewGuid();

            Transaction transaction = new Transaction(Guid.NewGuid());
            transaction.WalletId = walletdto.Id;
            transaction.TransactionType = Domain.Primitives.Enum.TransactionTypeEnum.Credit;
            transaction.TransactionReference = TransactionReference.ToString();
            transaction.Description = "ADVERT REQUEST";
            transaction.Status = Domain.Primitives.Enum.EntityStatus.Pending;
            transaction.UserId = companyDetailsDto.UserId;
            transaction.Amount = request.AdvertRequest.Amount;
            //transaction.Note = "ADVERT REQUEST";
            //transaction.Note = "ADVERT REQUEST";

                // Persist to the database
                Guid transactionId = await _transactionRepository.InsertReturnIdAsync(transaction);

           AdvertRequest newAdvert = new AdvertRequest();

            try
            {

                var xresult = await _storage.MainUploadFileReturnUrlAsync("", request.File);
                // 
                if (xresult.Message.Contains("200"))
                {
                    newAdvert.ImageUrl = xresult.Url;
                    newAdvert.ImageKey = xresult.Key;
                }

            }
            catch (Exception c)
            {

            }
            // Persist to the database
            newAdvert.Amount = request.AdvertRequest.Amount;
            newAdvert.Note = request.AdvertRequest.Note;
            newAdvert.CreatedAtUtc = DateTime.UtcNow.AddHours(1);
            newAdvert.Status = Domain.Primitives.Enum.EntityStatus.Pending;
            newAdvert.CompanyId = companyDetailsDto.Id;
            newAdvert.TransactionReference = TransactionReference.ToString();
            await _advertRequestRepository.InsertAsync(newAdvert);

            // Remove the cache
            await _advertRequestCacheHandler.RemoveListAsync();
            await _advertRequestCacheHandler.RemoveGetAsync(request.AdvertRequest.Id);
            await _advertRequestCacheHandler.RemoveDetailsByIdAsync(request.AdvertRequest.Id);
            await _advertRequestCacheHandler.RemoveList10ByCompanyAsync(request.AdvertRequest.CompanyId);
            // Remove the cache
            await _transactionCacheHandler.RemoveListAsync();
            await _transactionCacheHandler.RemoveGetAsync(transaction.Id);
            await _transactionCacheHandler.RemoveDetailsByIdAsync(transaction.Id);
            await _transactionCacheHandler.RemoveList10ByUserAsync(transaction.UserId);


                var setting = await _settingRepository.GetSettingAsync();
                if(setting.PaymentGateway == Domain.Primitives.Enum.PaymentGateway.Bank)
                {
                    responseData.PaymentGateWay = Domain.Primitives.Enum.PaymentGateway.Bank;
                    responseData.BankName = setting.BankName;
                    responseData.BankAccountNumber = setting.BankAccountNumber;
                    responseData.BankAccount = setting.BankAccount;

                    return responseData;
                }
                else
                {

                


            DataDeviceInformation dataDeviceInformation = new DataDeviceInformation();
            //run the flutterwave transaction
            var httpContext = _httpContextAccessor.HttpContext;

            PaymentRequestDto model = new PaymentRequestDto();
            model.Currency = "NGN";
            model.TxRef = transactionId.ToString();
            model.Amount = request.AdvertRequest.Amount.ToString();
            model.PaymentOptions = "card";
            model.ConsumerId = companyDetailsDto.UserId;
            model.ConsumerMac = dataDeviceInformation.GetMacAddress();
            model.Email = companyDetailsDto.Email;
            model.PhoneNumber = companyDetailsDto.Phone;
            model.Name = companyDetailsDto.Fullname;
            model.Title = "Koboview";
            model.Description = "kv";
            model.Logo = "";
            model.RedirectUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/Payment/verify?areapath=client";

            FlutterResponseDto dataResponse = await _repositoryService.InitializeTransaction(model);

                responseData.FlutterResponseDto = dataResponse;
                    responseData.PaymentGateWay = Domain.Primitives.Enum.PaymentGateway.Flutterwave;

                    return responseData;
                }
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
