using System.Linq.Expressions;
using JurayKV.Application.Caching.Repositories;
using JurayKV.Domain.Aggregates.NotificationAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.NotificationQueries;

public sealed class GetNotificationByIdQuery : IRequest<NotificationDto>
{
    public GetNotificationByIdQuery(Guid id)
    {
        Id = id.ThrowIfEmpty(nameof(id));
    }

    public Guid Id { get; }

    // Handler
    private class GetNotificationByIdQueryHandler : IRequestHandler<GetNotificationByIdQuery, NotificationDto>
    {
        private readonly INotificationCacheRepository _notificationCacheRepository;

        public GetNotificationByIdQueryHandler(IQueryRepository repository, INotificationCacheRepository notificationCacheRepository)
        {
            _notificationCacheRepository = notificationCacheRepository;
        }

        public async Task<NotificationDto> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

           
            NotificationDto notificationDetailsDto = await _notificationCacheRepository.GetByIdAsync(request.Id);

            return notificationDetailsDto;
        }
    }
}

 