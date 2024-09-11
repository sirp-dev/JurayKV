using JurayKV.Application.Caching.Repositories;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.NotificationQueries;

public sealed class GetNotificationListQuery : IRequest<List<NotificationDto>>
{
    private class GetNotificationListQueryHandler : IRequestHandler<GetNotificationListQuery, List<NotificationDto>>
    {
        private readonly INotificationCacheRepository _notificationCacheRepository;

        public GetNotificationListQueryHandler(INotificationCacheRepository notificationCacheRepository)
        {
            _notificationCacheRepository = notificationCacheRepository;
        }

        public async Task<List<NotificationDto>> Handle(GetNotificationListQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            List<NotificationDto> notificationDtos = await _notificationCacheRepository.GetListAsync();
            return notificationDtos;
        }
    }
}

 