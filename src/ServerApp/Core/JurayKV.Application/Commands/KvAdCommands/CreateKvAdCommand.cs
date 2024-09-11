using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Infrastructures;
using JurayKV.Application.Services.AwsDtos;
using JurayKV.Domain.Aggregates.IdentityKvAdAggregate;
using JurayKV.Domain.Aggregates.KvAdAggregate;
using JurayKV.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.IO;
using TanvirArjel.ArgumentChecker;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Commands.KvAdCommands;

public sealed class CreateKvAdCommand : IRequest<Guid>
{
    public CreateKvAdCommand(Guid imageId, Guid userId, Guid bucketId, Guid companyId, DateTime date, DataStatus status)
    {
        ImageId = imageId;
        UserId = userId;
        BucketId = bucketId;
        CompanyId = companyId;
        Date = date;
        Status = status;
    }

    public Guid ImageId { get; set; }
    public Guid UserId { get; private set; }
    public Guid BucketId { get; private set; }
    public Guid CompanyId { get; private set; }
    public DateTime Date { get; private set; }
    public DataStatus Status { get; private set; }
}

internal class CreateKvAdCommandHandler : IRequestHandler<CreateKvAdCommand, Guid>
{
    private readonly IKvAdRepository _kvAdRepository;
    private readonly IKvAdCacheHandler _kvAdCacheHandler;
    private readonly IStorageService _storage;
    public CreateKvAdCommandHandler(
            IKvAdRepository kvAdRepository,
            IKvAdCacheHandler kvAdCacheHandler,
            IStorageService storage)
    {
        _kvAdRepository = kvAdRepository;
        _kvAdCacheHandler = kvAdCacheHandler;
        _storage = storage;
    }

    public async Task<Guid> Handle(CreateKvAdCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));
       
     
        var check = await _kvAdRepository.ExistsAsync(ad => ad.DateId == request.Date.ToString("ddMMyyyy") && ad.BucketId == request.BucketId);
        if(check == true)
        {
            return Guid.Empty;
        }
        KvAd create = new KvAd(Guid.NewGuid());
        create.BucketId = request.BucketId;
        create.UserId = request.UserId;
        create.CompanyId = request.CompanyId;
        create.ImageFileId = request.ImageId;
         create.Status = Domain.Primitives.Enum.DataStatus.Active;
        create.CreatedAtUtc = request.Date;
        create.DateId = request.Date.ToString("ddMMyyyy");
        create.Status = request.Status;
        // Persist to the database

        await _kvAdRepository.InsertAsync(create);

        // Remove the cache
        await _kvAdCacheHandler.RemoveListAsync();
        await _kvAdCacheHandler.RemoveDetailsByIdAsync(create.Id);
        await _kvAdCacheHandler.RemoveByBucketIdAsync(create.BucketId);
        await _kvAdCacheHandler.RemoveGetAsync(create.Id);
         

        return create.Id;
    }
}