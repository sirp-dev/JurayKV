using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Infrastructures;
using JurayKV.Domain.Aggregates.NotificationAggregate;
using JurayKV.Domain.Aggregates.SettingAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.NotificationCommands
{
   
public sealed class AddNotificationCommand : IRequest
    {
        public AddNotificationCommand(CreateNotificationDto data)
        {
            Data = data;

        }

        public CreateNotificationDto Data { get; }

    }

    internal class AddNotificationCommandHandler : IRequestHandler<AddNotificationCommand>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationCacheHandler _notificationCacheHandler;

        public AddNotificationCommandHandler(
INotificationRepository notificationRepository, INotificationCacheHandler notificationCacheHandler)
        {
            _notificationRepository = notificationRepository;
            _notificationCacheHandler = notificationCacheHandler;
        }

        public async Task Handle(AddNotificationCommand request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));

            Notification notification = new Notification();
            notification.UserId = request.Data.UserId;
            notification.NotificationType = request.Data.NotificationType;
            notification.NotificationStatus = Domain.Primitives.Enum.NotificationStatus.NotSent;
            notification.AddedAtUtc = DateTime.UtcNow.AddHours(1);
            notification.Message = request.Data.Message;
            notification.Subject = request.Data.Subject;
            await _notificationRepository.InsertAsync(notification);
             
            // Remove the cache
            await _notificationCacheHandler.RemoveListAsync(); 
        }
    }
}
