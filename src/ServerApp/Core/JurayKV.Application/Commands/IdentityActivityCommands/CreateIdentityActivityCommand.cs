using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.IdentityActivityAggregate;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.ValueObjects;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.IdentityActivityCommands;

public sealed class CreateIdentityActivityCommand : IRequest<Guid>
{
    public CreateIdentityActivityCommand(string activity, Guid userId)
    {
        Activity = activity;
        UserId = userId;
    }

    public Guid UserId { get; private set; }
    public string Activity { get; set; }
}

internal class CreateIdentityActivityCommandHandler : IRequestHandler<CreateIdentityActivityCommand, Guid>
{
    private readonly IIdentityActivityRepository _identityActivityRepository;
    private readonly IIdentityActivityCacheHandler _identityActivityCacheHandler;

    public CreateIdentityActivityCommandHandler(
            IIdentityActivityRepository identityActivityRepository,
            IIdentityActivityCacheHandler identityActivityCacheHandler)
    {
        _identityActivityRepository = identityActivityRepository;
        _identityActivityCacheHandler = identityActivityCacheHandler;
    }

    public async Task<Guid> Handle(CreateIdentityActivityCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));

        IdentityActivity identityActivity = new IdentityActivity(Guid.NewGuid());
        identityActivity.UserId = request.UserId;
        identityActivity.Activity = request.Activity;
 
        // Persist to the database
        await _identityActivityRepository.InsertAsync(identityActivity);

        // Remove the cache
        await _identityActivityCacheHandler.RemoveListAsync();
        await _identityActivityCacheHandler.RemoveGetAsync(identityActivity.Id);
        await _identityActivityCacheHandler.RemoveGetAsync(identityActivity.Id);

        return identityActivity.Id;
    }
}