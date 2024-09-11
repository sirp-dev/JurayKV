using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.KvPointAggregate;
using JurayKV.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.KvPointCommands;

public sealed class DeleteKvPointCommand : IRequest
{
    public DeleteKvPointCommand(Guid kvPointId)
    {
        Id = kvPointId.ThrowIfEmpty(nameof(kvPointId));
    }

    public Guid Id { get; }
}

internal class DeleteKvPointCommandHandler : IRequestHandler<DeleteKvPointCommand>
{
    private readonly IKvPointRepository _kvPointRepository;
    private readonly IKvPointCacheHandler _kvPointCacheHandler;

    public DeleteKvPointCommandHandler(IKvPointRepository kvPointRepository, IKvPointCacheHandler kvPointCacheHandler)
    {
        _kvPointRepository = kvPointRepository;
        _kvPointCacheHandler = kvPointCacheHandler;
    }

    public async Task Handle(DeleteKvPointCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));

        KvPoint kvPoint = await _kvPointRepository.GetByIdAsync(request.Id);

        if (kvPoint == null)
        {
            throw new EntityNotFoundException(typeof(KvPoint), request.Id);
        }

        await _kvPointRepository.DeleteAsync(kvPoint);
        // Remove the cache
        await _kvPointCacheHandler.RemoveListAsync();
        await _kvPointCacheHandler.RemoveDetailsByIdAsync(kvPoint.Id);
        await _kvPointCacheHandler.RemoveListBy10ByUserAsync(kvPoint.UserId);
        await _kvPointCacheHandler.RemoveListByUserAsync(kvPoint.UserId);
        await _kvPointCacheHandler.RemoveGetAsync(kvPoint.Id);
    }
}