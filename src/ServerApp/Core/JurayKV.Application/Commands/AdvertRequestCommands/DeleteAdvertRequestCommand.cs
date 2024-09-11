using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.AdvertRequestAggregate;
using JurayKV.Domain.Exceptions;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.AdvertRequestCommands;

public sealed class DeleteAdvertRequestCommand : IRequest
{
    public DeleteAdvertRequestCommand(Guid advertRequestId)
    {
        Id = advertRequestId.ThrowIfEmpty(nameof(advertRequestId));
    }

    public Guid Id { get; }
}

internal class DeleteAdvertRequestCommandHandler : IRequestHandler<DeleteAdvertRequestCommand>
{
    private readonly IAdvertRequestRepository _advertRequestRepository;
    private readonly IAdvertRequestCacheHandler _advertRequestCacheHandler;

    public DeleteAdvertRequestCommandHandler(IAdvertRequestRepository advertRequestRepository, IAdvertRequestCacheHandler advertRequestCacheHandler)
    {
        _advertRequestRepository = advertRequestRepository;
        _advertRequestCacheHandler = advertRequestCacheHandler;
    }

    public async Task Handle(DeleteAdvertRequestCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));

        AdvertRequest advertRequest = await _advertRequestRepository.GetByIdAsync(request.Id);

        if (advertRequest == null)
        {
            throw new EntityNotFoundException(typeof(AdvertRequest), request.Id);
        }

        await _advertRequestRepository.DeleteAsync(advertRequest);
        // Remove the cache
        await _advertRequestCacheHandler.RemoveListAsync();
        await _advertRequestCacheHandler.RemoveGetAsync(advertRequest.Id);
        await _advertRequestCacheHandler.RemoveDetailsByIdAsync(advertRequest.Id);
        await _advertRequestCacheHandler.RemoveList10ByCompanyAsync(advertRequest.CompanyId);
    }
}