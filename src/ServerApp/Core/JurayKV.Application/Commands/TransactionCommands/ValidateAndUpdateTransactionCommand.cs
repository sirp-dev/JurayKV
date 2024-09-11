using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.TransactionAggregate;
using JurayKV.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;
using TanvirArjel.ArgumentChecker;
using Microsoft.EntityFrameworkCore.Storage;
using TanvirArjel.EFCore.GenericRepository;
using System.Data;
using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.AdvertRequestQueries;
using JurayKV.Domain.Aggregates.AdvertRequestAggregate;
using JurayKV.Domain.Aggregates.WalletAggregate;
using Microsoft.AspNetCore.Http;
using JurayKV.Application.Commands.NotificationCommands;
using JurayKV.Application.Queries.CompanyQueries;

namespace JurayKV.Application.Commands.TransactionCommands
{

    public sealed class ValidateAndUpdateTransactionCommand : IRequest<TransactionResponseDto>
    {
        public ValidateAndUpdateTransactionCommand(Guid id, string transaction_id, EntityStatus status)
        {
            Id = id;
            Status = status;
            Transaction_id = transaction_id;
        }
        public Guid Id { get; set; }
        public string Transaction_id { get; set; }
        public EntityStatus Status { get; set; }
    }

    internal class ValidateAndUpdateTransactionCommandHandler : IRequestHandler<ValidateAndUpdateTransactionCommand, TransactionResponseDto>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionCacheHandler _transactionCacheHandler;
        private readonly IRepository _repository;
        private readonly IAdvertRequestCacheRepository _advertRequestCacheRepository;
        private readonly IAdvertRequestRepository _advertRequestRepository;
        private readonly IAdvertRequestCacheHandler _advertRequestCacheHandler;
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletCacheHandler _walletCacheHandler;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;


        public ValidateAndUpdateTransactionCommandHandler(
            ITransactionRepository transactionRepository,
            ITransactionCacheHandler transactionCacheHandler,
            IRepository repository,
            IAdvertRequestCacheRepository advertRequestCacheRepository,
            IAdvertRequestRepository advertRequestRepository,
            IAdvertRequestCacheHandler advertRequestCacheHandler,
            IWalletRepository walletRepository,
            IWalletCacheHandler walletCacheHandler,
            IHttpContextAccessor httpContextAccessor,
            IMediator mediator)
        {
            _transactionRepository = transactionRepository;
            _transactionCacheHandler = transactionCacheHandler;
            _repository = repository;
            _advertRequestCacheRepository = advertRequestCacheRepository;
            _advertRequestRepository = advertRequestRepository;
            _advertRequestCacheHandler = advertRequestCacheHandler;
            _walletRepository = walletRepository;
            _walletCacheHandler = walletCacheHandler;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
        }

        public async Task<TransactionResponseDto> Handle(ValidateAndUpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));
            IDbContextTransaction dbContextTransaction = await _repository
              .BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
            TransactionResponseDto responseResult = new TransactionResponseDto();
            try
            {
                Transaction transactionToBeUpdated = await _transactionRepository.GetByIdAsync(request.Id);

                if (transactionToBeUpdated == null)
                {
                    throw new EntityNotFoundException(typeof(Transaction), request.Id);
                }
                //update transcation status
                transactionToBeUpdated.TransactionVerificationId = request.Transaction_id;
                transactionToBeUpdated.Status = request.Status;

                await _transactionRepository.UpdateAsync(transactionToBeUpdated);

                // Remove the cache
                await _transactionCacheHandler.RemoveListAsync();
                await _transactionCacheHandler.RemoveGetAsync(transactionToBeUpdated.Id);
                await _transactionCacheHandler.RemoveDetailsByIdAsync(transactionToBeUpdated.Id);
                await _transactionCacheHandler.RemoveList10ByUserAsync(transactionToBeUpdated.UserId);
                ///
                //update advert request

                var advertRequest = await _advertRequestRepository.GetByTransactionReferenceAsync(transactionToBeUpdated.TransactionReference);
                if (advertRequest != null)
                {
                    responseResult.Area = "Client";
                    responseResult.Path = "/Account/AdvertRequestHistory";


                advertRequest.Status = request.Status;
                await _advertRequestRepository.UpdateAsync(advertRequest);
                // Remove the cache
                await _advertRequestCacheHandler.RemoveListAsync();
                await _advertRequestCacheHandler.RemoveGetAsync(advertRequest.Id);
                await _advertRequestCacheHandler.RemoveDetailsByIdAsync(advertRequest.Id);
                await _advertRequestCacheHandler.RemoveList10ByCompanyAsync(advertRequest.CompanyId);
                }
                //update wallet
                var getwallet = await _walletRepository.GetByUserIdAsync(transactionToBeUpdated.UserId);
                getwallet.Amount += Convert.ToDecimal(transactionToBeUpdated.Amount);
                getwallet.LastUpdateAtUtc = DateTime.UtcNow.AddHours(1);
                var loguserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                getwallet.Log =  "<li> Wallet Update from " + transactionToBeUpdated.Description+" " + advertRequest.Id + " ::Amount: " + transactionToBeUpdated.Amount + " ::Balance: " + getwallet.Amount + " :: Date: " + getwallet.LastUpdateAtUtc + ":: Loggedin User: " + loguserId+"</li>"+ getwallet.Log;
                //getwallet = null;
                await _walletRepository.UpdateAsync(getwallet);

                await _walletCacheHandler.RemoveListAsync();

                await _walletCacheHandler.RemoveDetailsByIdAsync(getwallet.Id);
                await _walletCacheHandler.RemoveDetailsByUserIdAsync(getwallet.UserId);
                await _walletCacheHandler.RemoveGetAsync(getwallet.Id);
                 
                //create Email
                CreateNotificationDto addmail = new CreateNotificationDto();
                addmail.UserId = transactionToBeUpdated.UserId;
                addmail.NotificationType = NotificationType.Email;
                addmail.Subject = "Transaction Successful";
                // Transfer confirmation template
                string mailTemplate = @"
Your advert request of ₦{0:N2} has been confirmed and the dashboard credited. <br>Your available Koboview Account balance is ₦{1:N2}.

";

                // Data to replace placeholders in the template
                decimal amount = transactionToBeUpdated.Amount;
                decimal balance = getwallet.Amount;

                // Format the template with the actual data using string interpolation
                string emailMessage = string.Format(mailTemplate, amount, balance);
                addmail.Message = emailMessage;
                AddNotificationCommand AddMailCommand = new AddNotificationCommand(addmail);
                  await _mediator.Send(AddMailCommand);

                responseResult.Success = true;
                await dbContextTransaction.CommitAsync(cancellationToken);
                return responseResult;
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync(cancellationToken);
                responseResult.Success = false;
                return responseResult;
            }
        }
    }
}
