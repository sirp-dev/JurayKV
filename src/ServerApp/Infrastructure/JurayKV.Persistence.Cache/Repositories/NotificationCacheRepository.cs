using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.NotificationQueries;
using JurayKV.Domain.Aggregates.NotificationAggregate;
using JurayKV.Persistence.Cache.Keys;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using TanvirArjel.Extensions.Microsoft.Caching;

namespace JurayKV.Persistence.Cache.Repositories
{
    public sealed class NotificationCacheRepository : INotificationCacheRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IQueryRepository _repository;

        public NotificationCacheRepository(IDistributedCache distributedCache, IQueryRepository repository)
        {
            _distributedCache = distributedCache;
            _repository = repository;
        }

        public async Task<List<NotificationDto>> GetListAsync()
        {
            string cacheKey = NotificationCacheKeys.ListKey;
            List<NotificationDto> list = await _distributedCache.GetAsync<List<NotificationDto>>(cacheKey);

            if (list == null)
            {
                Expression<Func<Notification, NotificationDto>> selectExp = d => new NotificationDto
                {
                    Id = d.Id,
                   UserId = d.UserId.ToString(),
                   AddedAtUtc = d.AddedAtUtc,
                   NotificationStatus = d.NotificationStatus,
                   NotificationType = d.NotificationType,
                   SentAtUtc = d.SentAtUtc,
                   ResentAtUtc = d.SentAtUtc,
                   Message = d.Message,
                };

                list = await _repository.GetListAsync(selectExp);

                await _distributedCache.SetAsync(cacheKey, list);
            }

            return list;
        }

        public async Task<NotificationDto> GetByIdAsync(Guid notificationId)
        {
            string cacheKey = NotificationCacheKeys.GetKey(notificationId);
            NotificationDto notification = await _distributedCache.GetAsync<NotificationDto>(cacheKey);

            if (notification == null)
            {
                Expression<Func<Notification, NotificationDto>> selectExp = d => new NotificationDto
                {
                    Id = d.Id,
                    UserId = d.UserId.ToString(),
                    AddedAtUtc = d.AddedAtUtc,
                    NotificationStatus = d.NotificationStatus,
                    NotificationType = d.NotificationType,
                    SentAtUtc = d.SentAtUtc,
                    ResentAtUtc = d.SentAtUtc,
                    Message = d.Message,
                };

                notification = await _repository.GetByIdAsync(notificationId, selectExp);

                await _distributedCache.SetAsync(cacheKey, notification);
            }

            return notification;
        }

        public async Task<NotificationDto> GetDetailsByIdAsync(Guid notificationId)
        {
            string cacheKey = NotificationCacheKeys.GetDetailsKey(notificationId);
            NotificationDto notification = await _distributedCache.GetAsync<NotificationDto>(cacheKey);

            if (notification == null)
            {
                Expression<Func<Notification, NotificationDto>> selectExp = d => new NotificationDto
                {
                    Id = d.Id,
                    UserId = d.UserId.ToString(),
                    AddedAtUtc = d.AddedAtUtc,
                    NotificationStatus = d.NotificationStatus,
                    NotificationType = d.NotificationType,
                    SentAtUtc = d.SentAtUtc,
                    ResentAtUtc = d.SentAtUtc,
                    Message = d.Message,
                };

                notification = await _repository.GetByIdAsync(notificationId, selectExp);

                await _distributedCache.SetAsync(cacheKey, notification);
            }

            return notification;
        }
    }

}
