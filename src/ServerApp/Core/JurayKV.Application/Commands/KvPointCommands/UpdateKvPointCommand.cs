using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.KvPointAggregate;
using JurayKV.Domain.Exceptions;
using JurayKV.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using TanvirArjel.ArgumentChecker;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Commands.KvPointCommands;

public sealed class UpdateKvPointCommand : IRequest
{
    public UpdateKvPointCommand(Guid id, Guid userId, Guid identityKvAdId, EntityStatus status, int point, string pointHash)
    {
        Id = id;
        UserId = userId;
        IdentityKvAdId = identityKvAdId;
        Status = status;
        Point = point;
        PointHash = pointHash;
    }

    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid IdentityKvAdId { get; set; }
    public EntityStatus Status { get; set; }

    public int Point { get; set; }
    public string PointHash { get; set; }
}

internal class UpdateKvPointCommandHandler : IRequestHandler<UpdateKvPointCommand>
{
    private readonly IKvPointRepository _kvPointRepository;
    private readonly IKvPointCacheHandler _kvPointCacheHandler;

    public UpdateKvPointCommandHandler(
        IKvPointRepository kvPointRepository,
        IKvPointCacheHandler kvPointCacheHandler)
    {
        _kvPointRepository = kvPointRepository;
        _kvPointCacheHandler = kvPointCacheHandler;
    }

    public async Task Handle(UpdateKvPointCommand request, CancellationToken cancellationToken)
    {
        request.ThrowIfNull(nameof(request));

        KvPoint kvPointToBeUpdated = await _kvPointRepository.GetByIdAsync(request.Id);

        if (kvPointToBeUpdated == null)
        {
            throw new EntityNotFoundException(typeof(KvPoint), request.Id);
        }
        kvPointToBeUpdated.UserId = request.UserId;
        kvPointToBeUpdated.IdentityKvAdId = request.IdentityKvAdId;
        kvPointToBeUpdated.Status = request.Status;
        kvPointToBeUpdated.Point = request.Point;
        kvPointToBeUpdated.PointHash = request.PointHash;
        kvPointToBeUpdated.LastModifiedAtUtc= DateTime.UtcNow;

        await _kvPointRepository.UpdateAsync(kvPointToBeUpdated);

        // Remove the cache
        await _kvPointCacheHandler.RemoveListAsync();
        await _kvPointCacheHandler.RemoveDetailsByIdAsync(kvPointToBeUpdated.Id);
        await _kvPointCacheHandler.RemoveListBy10ByUserAsync(kvPointToBeUpdated.UserId);
        await _kvPointCacheHandler.RemoveGetAsync(kvPointToBeUpdated.Id);

        await _kvPointCacheHandler.RemoveListByUserAsync(kvPointToBeUpdated.UserId);
    }
}
