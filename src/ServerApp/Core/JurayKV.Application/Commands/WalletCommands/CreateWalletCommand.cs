using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.WalletAggregate;
using JurayKV.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.WalletCommands;

public sealed class CreateWalletCommand : IRequest<Guid>
{
    public CreateWalletCommand(Guid userId, string note, string log, decimal amount)
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

internal class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, Guid>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletCacheHandler _walletCacheHandler;

    public CreateWalletCommandHandler(
            IWalletRepository walletRepository,
            IWalletCacheHandler walletCacheHandler)
    {
        _walletRepository = walletRepository;
        _walletCacheHandler = walletCacheHandler;
    }

    public async Task<Guid> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));

        
        Wallet wallet = new Wallet(Guid.NewGuid());
        wallet.UserId = request.UserId;
        wallet.Note = request.Note;
        wallet.Log = request.Log;
        wallet.Amount = request.Amount;
        // Persist to the database
        await _walletRepository.InsertAsync(wallet);

        // Remove the cache
        await _walletCacheHandler.RemoveListAsync();
         
        await _walletCacheHandler.RemoveDetailsByIdAsync(wallet.Id);
        await _walletCacheHandler.RemoveDetailsByUserIdAsync(wallet.UserId);
        await _walletCacheHandler.RemoveGetAsync(wallet.Id);
        return wallet.Id;
    }
}