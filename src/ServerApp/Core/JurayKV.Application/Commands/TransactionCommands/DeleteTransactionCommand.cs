using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.TransactionAggregate;
using JurayKV.Domain.Exceptions;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.TransactionCommands;

public sealed class DeleteTransactionCommand : IRequest
{
    public DeleteTransactionCommand(Guid transactionId)
    {
        Id = transactionId.ThrowIfEmpty(nameof(transactionId));
    }

    public Guid Id { get; }
}

internal class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ITransactionCacheHandler _transactionCacheHandler;

    public DeleteTransactionCommandHandler(ITransactionRepository transactionRepository, ITransactionCacheHandler transactionCacheHandler)
    {
        _transactionRepository = transactionRepository;
        _transactionCacheHandler = transactionCacheHandler;
    }

    public async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));

        Transaction transaction = await _transactionRepository.GetByIdAsync(request.Id);

        if (transaction == null)
        {
            throw new EntityNotFoundException(typeof(Transaction), request.Id);
        }

        await _transactionRepository.DeleteAsync(transaction);
        // Remove the cache
        await _transactionCacheHandler.RemoveListAsync();
        await _transactionCacheHandler.RemoveGetAsync(transaction.Id);
        await _transactionCacheHandler.RemoveDetailsByIdAsync(transaction.Id);
        await _transactionCacheHandler.RemoveList10ByUserAsync(transaction.UserId);
    }
}