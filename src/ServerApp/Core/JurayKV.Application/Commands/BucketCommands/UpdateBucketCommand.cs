using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.BucketAggregate;
using JurayKV.Domain.Exceptions;
using JurayKV.Domain.ValueObjects;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.BucketCommands;

public sealed class UpdateBucketCommand : IRequest
{
    public UpdateBucketCommand(
        Guid id,
        string name,
        bool disable,
        bool adminActive,
        bool userActive)
    {
        Id = id;
        Name = name;
        Disable = disable;
        AdminActive = adminActive;
        UserActive = userActive;
    }

    public Guid Id { get; }

    public string Name { get; set; }

    public bool Disable { get; set; }

    public bool AdminActive { get; set; }

    public bool UserActive { get; set; }
}

internal class UpdateBucketCommandHandler : IRequestHandler<UpdateBucketCommand>
{
    private readonly IBucketRepository _bucketRepository;
    private readonly IBucketCacheHandler _bucketCacheHandler;

    public UpdateBucketCommandHandler(
        IBucketRepository bucketRepository,
        IBucketCacheHandler bucketCacheHandler)
    {
        _bucketRepository = bucketRepository;
        _bucketCacheHandler = bucketCacheHandler;
    }

    public async Task Handle(UpdateBucketCommand request, CancellationToken cancellationToken)
    {
        request.ThrowIfNull(nameof(request));

        Bucket bucketToBeUpdated = await _bucketRepository.GetByIdAsync(request.Id);

        if (bucketToBeUpdated == null)
        {
            throw new EntityNotFoundException(typeof(Bucket), request.Id);
        }

        bucketToBeUpdated.Name = request.Name;
        bucketToBeUpdated.Disable = request.Disable;
        bucketToBeUpdated.AdminActive = request.AdminActive;
        bucketToBeUpdated.UserActive = request.UserActive;

        await _bucketRepository.UpdateAsync(bucketToBeUpdated);

        await _bucketCacheHandler.RemoveListAsync();
        await _bucketCacheHandler.RemoveDropdownListAsync();
        await _bucketCacheHandler.RemoveDetailsByIdAsync(bucketToBeUpdated.Id);
        await _bucketCacheHandler.RemoveGetByIdAsync(bucketToBeUpdated.Id);

    }
}
