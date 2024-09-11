using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Infrastructures;
using JurayKV.Domain.Aggregates.KvAdAggregate;
using JurayKV.Domain.Exceptions;
using JurayKV.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using TanvirArjel.ArgumentChecker;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Commands.KvAdCommands;

public sealed class UpdateKvAdCommand : IRequest
{
    public UpdateKvAdCommand(Guid id, Guid? imageId, Guid userId, Guid bucketId, Guid companyId, DataStatus status)
    {
        Id = id;
        ImageId = imageId;
        UserId = userId;
        BucketId = bucketId;
        CompanyId = companyId;
        Status = status;
    }
    public Guid Id { get; set; }
    public Guid? ImageId { get; set; }
    public Guid UserId { get;   set; }
    public Guid BucketId { get;   set; }
    public Guid CompanyId { get;   set; }
    public DataStatus Status { get;   set; }
}

internal class UpdateKvAdCommandHandler : IRequestHandler<UpdateKvAdCommand>
{
    private readonly IKvAdRepository _kvAdRepository;
    private readonly IKvAdCacheHandler _kvAdCacheHandler;
    private readonly IStorageService _storage;

    public UpdateKvAdCommandHandler(
        IKvAdRepository kvAdRepository,
        IKvAdCacheHandler kvAdCacheHandler,
        IStorageService storage)
    {
        _kvAdRepository = kvAdRepository;
        _kvAdCacheHandler = kvAdCacheHandler;
        _storage = storage;
    }

    public async Task Handle(UpdateKvAdCommand request, CancellationToken cancellationToken)
    {
        request.ThrowIfNull(nameof(request));

        KvAd kvAdToBeUpdated = await _kvAdRepository.GetByIdAsync(request.Id);

        if (kvAdToBeUpdated == null)
        {
            throw new EntityNotFoundException(typeof(KvAd), request.Id);
        }
       
         
        kvAdToBeUpdated.BucketId = request.BucketId;
        kvAdToBeUpdated.UserId = request.UserId;
        if(request.ImageId != null) { 
        kvAdToBeUpdated.ImageFileId = request.ImageId;
        }
         kvAdToBeUpdated.Status = request.Status;

        await _kvAdRepository.UpdateAsync(kvAdToBeUpdated);

        // Remove the cache
        await _kvAdCacheHandler.RemoveListAsync();
        await _kvAdCacheHandler.RemoveDetailsByIdAsync(kvAdToBeUpdated.Id);
        await _kvAdCacheHandler.RemoveByBucketIdAsync(kvAdToBeUpdated.BucketId);
        await _kvAdCacheHandler.RemoveGetAsync(kvAdToBeUpdated.Id);

    }
}
