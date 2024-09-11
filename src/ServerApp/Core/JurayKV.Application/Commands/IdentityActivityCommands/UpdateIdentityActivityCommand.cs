using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.IdentityActivityAggregate;
using JurayKV.Domain.Exceptions;
using JurayKV.Domain.ValueObjects;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.IdentityActivityCommands;

public sealed class UpdateIdentityActivityCommand : IRequest
{
    public UpdateIdentityActivityCommand(
        Guid userId,
        string activity
        )
    {
        UserId = userId;
        Activity = activity;

    }

    public Guid UserId { get; }

    public string Activity { get; }
}

internal class UpdateIdentityActivityCommandHandler : IRequestHandler<UpdateIdentityActivityCommand>
{
    private readonly IIdentityActivityRepository _identityActivityRepository;
    private readonly IIdentityActivityCacheHandler _identityActivityCacheHandler;

    public UpdateIdentityActivityCommandHandler(
        IIdentityActivityRepository identityActivityRepository,
        IIdentityActivityCacheHandler identityActivityCacheHandler)
    {
        _identityActivityRepository = identityActivityRepository;
        _identityActivityCacheHandler = identityActivityCacheHandler;
    }

    public async Task Handle(UpdateIdentityActivityCommand request, CancellationToken cancellationToken)
    {
        request.ThrowIfNull(nameof(request));

        IdentityActivity identityActivityToBeUpdated = await _identityActivityRepository.GetByUserIdAsync(request.UserId);

        if (identityActivityToBeUpdated == null)
        {
            throw new EntityNotFoundException(typeof(IdentityActivity), request.UserId);
        }

        identityActivityToBeUpdated.Activity = identityActivityToBeUpdated.Activity + "br" + request.Activity;

        await _identityActivityRepository.UpdateAsync(identityActivityToBeUpdated);

        // Remove the cache
        await _identityActivityCacheHandler.RemoveListAsync();
        await _identityActivityCacheHandler.RemoveGetAsync(identityActivityToBeUpdated.Id);
        await _identityActivityCacheHandler.RemoveGetAsync(identityActivityToBeUpdated.Id);
    }
}
