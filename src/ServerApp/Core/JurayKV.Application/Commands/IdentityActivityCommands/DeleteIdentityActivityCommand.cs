using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.IdentityActivityAggregate;
using JurayKV.Domain.Exceptions;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.IdentityActivityCommands;

public sealed class DeleteIdentityActivityCommand : IRequest
{
    public DeleteIdentityActivityCommand(Guid identityActivityId)
    {
        Id = identityActivityId.ThrowIfEmpty(nameof(identityActivityId));
    }

    public Guid Id { get; }
}

internal class DeleteIdentityActivityCommandHandler : IRequestHandler<DeleteIdentityActivityCommand>
{
    private readonly IIdentityActivityRepository _identityActivityRepository;
    private readonly IIdentityActivityCacheHandler _identityActivityCacheHandler;

    public DeleteIdentityActivityCommandHandler(IIdentityActivityRepository identityActivityRepository, IIdentityActivityCacheHandler identityActivityCacheHandler)
    {
        _identityActivityRepository = identityActivityRepository;
        _identityActivityCacheHandler = identityActivityCacheHandler;
    }

    public async Task Handle(DeleteIdentityActivityCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));

        IdentityActivity identityActivity = await _identityActivityRepository.GetByIdAsync(request.Id);

        if (identityActivity == null)
        {
            throw new EntityNotFoundException(typeof(IdentityActivity), request.Id);
        }

        await _identityActivityRepository.DeleteAsync(identityActivity);
        // Remove the cache
        await _identityActivityCacheHandler.RemoveListAsync();
        await _identityActivityCacheHandler.RemoveGetAsync(identityActivity.Id);
        await _identityActivityCacheHandler.RemoveGetAsync(identityActivity.Id);
    }
}