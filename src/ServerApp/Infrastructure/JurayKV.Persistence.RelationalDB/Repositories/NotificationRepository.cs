using JurayKV.Domain.Aggregates.NotificationAggregate;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using JurayKV.Domain.Aggregates.KvPointAggregate;
using JurayKV.Persistence.RelationalDB.Repositories.GenericRepositories;

namespace JurayKV.Persistence.RelationalDB.Repositories
{
    internal sealed class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly JurayDbContext _dbContext;

        public NotificationRepository(JurayDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> ExistsAsync(Expression<Func<Notification, bool>> condition)
        {
            IQueryable<Notification> queryable = _dbContext.Set<Notification>();

            if (condition != null)
            {
                queryable = queryable.Where(condition);
            }

            return queryable.AnyAsync();
        }

        public async Task<Notification> GetByIdAsync(Guid notificationId)
        {
            notificationId.ThrowIfEmpty(nameof(notificationId));

            Notification notification = await _dbContext.Set<Notification>().FindAsync(notificationId);
            return notification;
        }

        public async Task InsertAsync(Notification notification)
        {
            notification.ThrowIfNull(nameof(notification));

            await _dbContext.AddAsync(notification);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Notification notification)
        {
            notification.ThrowIfNull(nameof(notification));

            EntityEntry<Notification> trackedEntity = _dbContext.ChangeTracker.Entries<Notification>()
                .FirstOrDefault(x => x.Entity == notification);

            if (trackedEntity == null)
            {
                _dbContext.Update(notification);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Notification notification)
        {
            notification.ThrowIfNull(nameof(notification));

            _dbContext.Remove(notification);
            await _dbContext.SaveChangesAsync();
        }
    }

}
