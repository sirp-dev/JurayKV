using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.WalletAggregate;
using JurayKV.Domain.Exceptions;
using JurayKV.Domain.ValueObjects;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.WalletCommands;

public sealed class UpdateWalletCommand : IRequest
{
    public UpdateWalletCommand(Guid userId, string note, string log, decimal amount)
    {
        UserId = userId;
        Note = note;
        Log = log;
        Amount = amount;

    }

    public Guid UserId { get; set; }
    public string Note { get; set; }
    public string Log { get; set; }
    public decimal Amount { get; set; }
}

internal class UpdateWalletCommandHandler : IRequestHandler<UpdateWalletCommand>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletCacheHandler _walletCacheHandler;

    public UpdateWalletCommandHandler(
        IWalletRepository walletRepository,
        IWalletCacheHandler walletCacheHandler)
    {
        _walletRepository = walletRepository;
        _walletCacheHandler = walletCacheHandler;
    }

    public async Task Handle(UpdateWalletCommand request, CancellationToken cancellationToken)
    {
        request.ThrowIfNull(nameof(request));

        Wallet walletToBeUpdated = await _walletRepository.GetByUserIdAsync(request.UserId);

        if (walletToBeUpdated == null)
        {
            throw new EntityNotFoundException(typeof(Wallet), request.UserId);
        }
         
        walletToBeUpdated.Amount = request.Amount;
        walletToBeUpdated.Note = request.Note;
        walletToBeUpdated.Log = request.Log;
        walletToBeUpdated.LastUpdateAtUtc = DateTime.UtcNow;

        await _walletRepository.UpdateAsync(walletToBeUpdated);

        await _walletCacheHandler.RemoveListAsync();
        await _walletCacheHandler.RemoveDetailsByUserIdAsync(walletToBeUpdated.UserId);
        await _walletCacheHandler.RemoveDetailsByIdAsync(walletToBeUpdated.Id);
         
        await _walletCacheHandler.RemoveGetAsync(walletToBeUpdated.Id);
    }
}
