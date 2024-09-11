namespace JurayKV.Application.Caching.Repositories
{
    using JurayKV.Application.Queries.NotificationQueries;
    using JurayKV.Domain.Aggregates.NotificationAggregate;
    using TanvirArjel.Extensions.Microsoft.DependencyInjection;

    [ScopedService]
    public interface INotificationCacheRepository
    {
        Task<List<NotificationDto>> GetListAsync();

        Task<NotificationDto> GetByIdAsync(Guid modelId);

        Task<NotificationDto> GetDetailsByIdAsync(Guid modelId);
    }
}
