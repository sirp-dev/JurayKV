using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.TransactionAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;
using TanvirArjel.ArgumentChecker;
using JurayKV.Application.Queries.KvAdQueries;
using JurayKV.Domain.Aggregates.KvAdAggregate;
using System.Transactions;
using JurayKV.Application.Queries.WalletQueries;
using JurayKV.Application.Queries.TransactionQueries;
using JurayKV.Domain.Aggregates.AdvertRequestAggregate;
using JurayKV.Domain.Aggregates.WalletAggregate;
using Microsoft.AspNetCore.Http;
using JurayKV.Application.Commands.WalletCommands;

namespace JurayKV.Application.Commands.TransactionCommands
{
 

public sealed class CreateReconsileTransactionCommand : IRequest<bool>
    {
        public CreateReconsileTransactionCommand(Guid walletId,
            Guid userId,
            string uniqueReference,
            string optionalNote,
            decimal amount,
            TransactionTypeEnum transactionType,
            EntityStatus status,
            string transactionReference,
            string description,
            string trackCode)
        {
            WalletId = walletId;
            UserId = userId;
           UniqueReference = uniqueReference;
            OptionalNote = optionalNote;
            Amount = amount;
            TransactionType = transactionType;
            Status = status;
            TransactionReference = transactionReference;
            Description = description;
            TrackCode = trackCode;


        }

        public Guid WalletId { get; set; }
        public Guid UserId { get; set; }

        public string UniqueReference { get; set; }
        public string OptionalNote { get; set; }
        public decimal Amount { get; set; }

        public TransactionTypeEnum TransactionType { get; set; }
        public EntityStatus Status { get; set; }
        public string TransactionReference { get; set; }
        public string Description { get; set; }
        public string TrackCode { get; set; }
    }

    internal class CreateReconsileTransactionCommandHandler : IRequestHandler<CreateReconsileTransactionCommand, bool>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionCacheHandler _transactionCacheHandler;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWalletRepository _walletRepository;


        public CreateReconsileTransactionCommandHandler(
                ITransactionRepository transactionRepository,
                ITransactionCacheHandler transactionCacheHandler,
                IMediator mediator,
                IHttpContextAccessor httpContextAccessor,
                IWalletRepository walletRepository)
        {
            _transactionRepository = transactionRepository;
            _transactionCacheHandler = transactionCacheHandler;
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
            _walletRepository = walletRepository;
        }

        public async Task<bool> Handle(CreateReconsileTransactionCommand request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));

            CreateTransactionCommand transactioncreatecommand = new CreateTransactionCommand(request.WalletId, request.UserId, request.UniqueReference, request.OptionalNote, request.Amount,
                request.TransactionType, request.Status, request.TransactionReference, request.Description, request.TrackCode + "-RRR");

            var transaction = await _mediator.Send(transactioncreatecommand);

            //get the transaction by id
            GetTransactionByIdQuery gettranCommand = new GetTransactionByIdQuery(transaction);
            var thetransaction = await _mediator.Send(gettranCommand);

            //if transaction is debit.
            GetWalletUserByIdQuery walletcommand = new GetWalletUserByIdQuery(request.UserId);
            var getwallet = await _mediator.Send(walletcommand);
            if(thetransaction.TransactionType == TransactionTypeEnum.Credit)
            {
                getwallet.Amount = getwallet.Amount + thetransaction.Amount;

            }
            else if (thetransaction.TransactionType == TransactionTypeEnum.Debit)

            {
                getwallet.Amount = getwallet.Amount - thetransaction.Amount;

            }
             var loguserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            getwallet.Log =  "<li>RRR- Wallet Update from " + thetransaction.Description + " " + thetransaction.Id + " ::Amount: " + thetransaction.Amount + " ::Balance: " + getwallet.Amount + " :: Date: " + getwallet.LastUpdateAtUtc + ":: Loggedin User: " + loguserId + " </li>" + getwallet.Log;
            //getwallet = null;
            UpdateWalletCommand updatewalletcommand = new UpdateWalletCommand(getwallet.UserId, "Validate Transaction", getwallet.Log, getwallet.Amount);
            await _mediator.Send(updatewalletcommand);

            return true;
        }
    }
}
