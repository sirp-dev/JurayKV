using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.BucketAggregate;
using JurayKV.Domain.Exceptions;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.BucketCommands;

public sealed class DeleteBucketCommand : IRequest
{
    public DeleteBucketCommand(Guid bucketId)
    {
        Id = bucketId.ThrowIfEmpty(nameof(bucketId));
    }

    public Guid Id { get; }
}

internal class DeleteBucketCommandHandler : IRequestHandler<DeleteBucketCommand>
{
    private readonly IBucketRepository _bucketRepository;
    private readonly IBucketCacheHandler _bucketCacheHandler;

    public DeleteBucketCommandHandler(IBucketRepository bucketRepository, IBucketCacheHandler bucketCacheHandler)
    {
        _bucketRepository = bucketRepository;
        _bucketCacheHandler = bucketCacheHandler;
    }

    public async Task Handle(DeleteBucketCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));

        Bucket bucket = await _bucketRepository.GetByIdAsync(request.Id);

        if (bucket == null)
        {
            throw new EntityNotFoundException(typeof(Bucket), request.Id);
        }

        await _bucketRepository.DeleteAsync(bucket);
         
        await _bucketCacheHandler.RemoveListAsync();
        await _bucketCacheHandler.RemoveDropdownListAsync();
        await _bucketCacheHandler.RemoveDetailsByIdAsync(bucket.Id);
        await _bucketCacheHandler.RemoveGetByIdAsync(bucket.Id);
    }
}