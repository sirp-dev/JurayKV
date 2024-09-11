using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.NotificationAggregate;
using JurayKV.Domain.Exceptions;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.NotificationCommands;

public sealed class DeleteNotificationCommand : IRequest
{
    public DeleteNotificationCommand(Guid notificationId)
    {
        Id = notificationId.ThrowIfEmpty(nameof(notificationId));
    }

    public Guid Id { get; }
}

internal class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationCacheHandler _notificationCacheHandler;

    public DeleteNotificationCommandHandler(INotificationRepository notificationRepository, INotificationCacheHandler notificationCacheHandler)
    {
        _notificationRepository = notificationRepository;
        _notificationCacheHandler = notificationCacheHandler;
    }

    public async Task Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));

        Notification notification = await _notificationRepository.GetByIdAsync(request.Id);

        if (notification == null)
        {
            throw new EntityNotFoundException(typeof(Notification), request.Id);
        }

        await _notificationRepository.DeleteAsync(notification);
        // Remove the cache
        await _notificationCacheHandler.RemoveListAsync();
        await _notificationCacheHandler.RemoveDetailsByIdAsync(notification.Id);
        await _notificationCacheHandler.RemoveGetAsync(notification.Id);
    }
}