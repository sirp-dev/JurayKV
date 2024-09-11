using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.BucketAggregate;
using JurayKV.Domain.ValueObjects;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.BucketCommands;

public sealed class CreateBucketCommand : IRequest<Guid>
{
    public CreateBucketCommand(string name)
    {
        Name = name;
    }

    public string Name { get; }
     
}

internal class CreateBucketCommandHandler : IRequestHandler<CreateBucketCommand, Guid>
{
    private readonly IBucketRepository _bucketRepository;
    private readonly IBucketCacheHandler _bucketCacheHandler;

    public CreateBucketCommandHandler(
            IBucketRepository bucketRepository,
            IBucketCacheHandler bucketCacheHandler)
    {
        _bucketRepository = bucketRepository;
        _bucketCacheHandler = bucketCacheHandler;
    }

    public async Task<Guid> Handle(CreateBucketCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));
        Bucket bucket = new Bucket(Guid.NewGuid());

        bucket.Name = request.Name;
        // Persist to the database
        await _bucketRepository.Create(bucket);

        // Remove the cache
        await _bucketCacheHandler.RemoveListAsync();
        await _bucketCacheHandler.RemoveDropdownListAsync();
        await _bucketCacheHandler.RemoveDetailsByIdAsync(bucket.Id);
        await _bucketCacheHandler.RemoveGetByIdAsync(bucket.Id);
        return bucket.Id;
    }
}