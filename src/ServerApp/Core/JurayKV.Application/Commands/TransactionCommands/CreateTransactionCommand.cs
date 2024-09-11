using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.TransactionAggregate;
using JurayKV.Domain.Aggregates.WalletAggregate;
using JurayKV.Domain.ValueObjects;
using MediatR;
using TanvirArjel.ArgumentChecker;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Commands.TransactionCommands;

public sealed class CreateTransactionCommand : IRequest<Guid>
{
    public CreateTransactionCommand(Guid walletId, 
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

internal class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Guid>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionCacheHandler _transactionCacheHandler;

    public CreateTransactionCommandHandler(
            ITransactionRepository transactionRepository,
            ITransactionCacheHandler transactionCacheHandler)
    {
        _transactionRepository = transactionRepository;
        _transactionCacheHandler = transactionCacheHandler;
    }

    public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));

        Transaction transaction = new Transaction(Guid.NewGuid());
        transaction.WalletId= request.WalletId;
        transaction.TransactionType= request.TransactionType;
        transaction.TransactionReference= request.TransactionReference;
        transaction.Description= request.Description;
        transaction.Status= request.Status;
        transaction.UserId= request.UserId;
        transaction.Amount= request.Amount;
        transaction.OptionalNote = request.OptionalNote;
        transaction.UniqueReference = request.UniqueReference;

        // Persist to the database
        await _transactionRepository.InsertAsync(transaction);

        // Remove the cache
        await _transactionCacheHandler.RemoveListAsync();
        await _transactionCacheHandler.RemoveGetAsync(transaction.Id);
        await _transactionCacheHandler.RemoveDetailsByIdAsync(transaction.Id);
        await _transactionCacheHandler.RemoveList10ByUserAsync(transaction.UserId);

        return transaction.Id;
    }
}