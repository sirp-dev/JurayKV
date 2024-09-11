using JurayKV.Domain.Aggregates.DepartmentAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.NotificationAggregate
{
    public interface INotificationRepository
    {
        Task<Notification> GetByIdAsync(Guid modelId);         

        Task InsertAsync(Notification model);

        Task UpdateAsync(Notification model);

        Task DeleteAsync(Notification model);
    }
}
