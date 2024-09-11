using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.KvAdAggregate;
using JurayKV.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.KvAdCommands;

public sealed class DeleteKvAdCommand : IRequest
{
    public DeleteKvAdCommand(Guid kvAdId)
    {
        Id = kvAdId.ThrowIfEmpty(nameof(kvAdId));
    }

    public Guid Id { get; }
}

internal class DeleteKvAdCommandHandler : IRequestHandler<DeleteKvAdCommand>
{
    private readonly IKvAdRepository _kvAdRepository;
    private readonly IKvAdCacheHandler _kvAdCacheHandler;

    public DeleteKvAdCommandHandler(IKvAdRepository kvAdRepository, IKvAdCacheHandler kvAdCacheHandler)
    {
        _kvAdRepository = kvAdRepository;
        _kvAdCacheHandler = kvAdCacheHandler;
    }

    public async Task Handle(DeleteKvAdCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));

        KvAd kvAd = await _kvAdRepository.GetByIdAsync(request.Id);

        if (kvAd == null)
        {
            throw new EntityNotFoundException(typeof(KvAd), request.Id);
        }

        await _kvAdRepository.DeleteAsync(kvAd);
        // Remove the cache
        await _kvAdCacheHandler.RemoveListAsync();
        await _kvAdCacheHandler.RemoveDetailsByIdAsync(kvAd.Id);
        await _kvAdCacheHandler.RemoveByBucketIdAsync(kvAd.BucketId);
        await _kvAdCacheHandler.RemoveGetAsync(kvAd.Id);
    }
}