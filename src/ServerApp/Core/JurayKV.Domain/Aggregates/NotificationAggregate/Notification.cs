using JurayKV.Domain.Aggregates.DepartmentAggregate;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Primitives;
using JurayKV.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Domain.Aggregates.NotificationAggregate
{
    public class Notification : AggregateRoot
    {
         

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public NotificationStatus NotificationStatus { get; set; }

        public NotificationType NotificationType { get; set; }

        public DateTime AddedAtUtc { get; set; }

        public DateTime? SentAtUtc { get; set; }
        public DateTime? ResentAtUtc { get; set; }

        public string Message { get;set; }
        public string Subject { get;set; }
    }
}
