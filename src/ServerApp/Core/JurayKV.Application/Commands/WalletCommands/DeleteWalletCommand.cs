using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.WalletAggregate;
using JurayKV.Domain.Exceptions;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.WalletCommands;

public sealed class DeleteWalletCommand : IRequest
{
    public DeleteWalletCommand(Guid walletId)
    {
        Id = walletId.ThrowIfEmpty(nameof(walletId));
    }

    public Guid Id { get; }
}

internal class DeleteWalletCommandHandler : IRequestHandler<DeleteWalletCommand>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IWalletCacheHandler _walletCacheHandler;

    public DeleteWalletCommandHandler(IWalletRepository walletRepository, IWalletCacheHandler walletCacheHandler)
    {
        _walletRepository = walletRepository;
        _walletCacheHandler = walletCacheHandler;
    }

    public async Task Handle(DeleteWalletCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));

        Wallet wallet = await _walletRepository.GetByIdAsync(request.Id);

        if (wallet == null)
        {
            throw new EntityNotFoundException(typeof(Wallet), request.Id);
        }

        await _walletRepository.DeleteAsync(wallet);
        await _walletCacheHandler.RemoveListAsync();
        await _walletCacheHandler.RemoveDetailsByUserIdAsync(wallet.UserId);
        await _walletCacheHandler.RemoveDetailsByIdAsync(wallet.Id);
    }
}