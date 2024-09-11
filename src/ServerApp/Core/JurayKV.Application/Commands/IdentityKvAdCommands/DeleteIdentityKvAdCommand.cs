using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.IdentityKvAdAggregate;
using JurayKV.Domain.Exceptions;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.IdentityKvAdCommands;

public sealed class DeleteIdentityKvAdCommand : IRequest
{
    public DeleteIdentityKvAdCommand(Guid identityKvAdId)
    {
        Id = identityKvAdId.ThrowIfEmpty(nameof(identityKvAdId));
    }

    public Guid Id { get; }
}

internal class DeleteIdentityKvAdCommandHandler : IRequestHandler<DeleteIdentityKvAdCommand>
{
    private readonly IIdentityKvAdRepository _identityKvAdRepository;
    private readonly IIdentityKvAdCacheHandler _identityKvAdCacheHandler;

    public DeleteIdentityKvAdCommandHandler(IIdentityKvAdRepository identityKvAdRepository, IIdentityKvAdCacheHandler identityKvAdCacheHandler)
    {
        _identityKvAdRepository = identityKvAdRepository;
        _identityKvAdCacheHandler = identityKvAdCacheHandler;
    }

    public async Task Handle(DeleteIdentityKvAdCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));

        IdentityKvAd identityKvAd = await _identityKvAdRepository.GetByIdAsync(request.Id);

        if (identityKvAd == null)
        {
            throw new EntityNotFoundException(typeof(IdentityKvAd), request.Id);
        }

        await _identityKvAdRepository.DeleteAsync(identityKvAd);
        
        // Remove the cache
        await _identityKvAdCacheHandler.RemoveListAsync();
        await _identityKvAdCacheHandler.RemoveGetByUserIdAsync(identityKvAd.UserId);
        await _identityKvAdCacheHandler.RemoveGetActiveByUserIdAsync(identityKvAd.UserId);
        await _identityKvAdCacheHandler.RemoveDetailsByIdAsync(identityKvAd.Id);
        await _identityKvAdCacheHandler.RemoveGetAsync(identityKvAd.Id);
    }
}