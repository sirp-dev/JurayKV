using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.NotificationAggregate;
using JurayKV.Domain.Exceptions;
using JurayKV.Domain.ValueObjects;
using MediatR;
using TanvirArjel.ArgumentChecker;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Commands.NotificationCommands;

public sealed class UpdateNotificationCommand : IRequest
{
    public UpdateNotificationCommand(
        Guid id,
        NotificationStatus notificationStatus,
         DateTime? resentAtUtc)
    {
        Id = id;
        NotificationStatus = notificationStatus;
        ResentAtUtc = resentAtUtc;
     }

    public Guid Id { get; set; }

    public string UserId { get; set; }

    public NotificationStatus NotificationStatus { get; set; }
     
    public DateTime? ResentAtUtc { get; set; }
}

internal class UpdateNotificationCommandHandler : IRequestHandler<UpdateNotificationCommand>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationCacheHandler _notificationCacheHandler;

    public UpdateNotificationCommandHandler(
        INotificationRepository notificationRepository,
        INotificationCacheHandler notificationCacheHandler)
    {
        _notificationRepository = notificationRepository;
        _notificationCacheHandler = notificationCacheHandler;
    }

    public async Task Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
    {
        request.ThrowIfNull(nameof(request));

        Notification notificationToBeUpdated = await _notificationRepository.GetByIdAsync(request.Id);

        if (notificationToBeUpdated == null)
        {
            throw new EntityNotFoundException(typeof(Notification), request.Id);
        }

        notificationToBeUpdated.ResentAtUtc = request.ResentAtUtc;
        notificationToBeUpdated.NotificationStatus = request.NotificationStatus;
        await _notificationRepository.UpdateAsync(notificationToBeUpdated);

        // Remove the cache
        await _notificationCacheHandler.RemoveListAsync();
        await _notificationCacheHandler.RemoveDetailsByIdAsync(notificationToBeUpdated.Id);
        await _notificationCacheHandler.RemoveGetAsync(notificationToBeUpdated.Id);
    }
}
