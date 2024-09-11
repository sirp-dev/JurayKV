using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Queries.NotificationQueries
{
    public class NotificationDto
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public NotificationStatus NotificationStatus { get; set; }

        public NotificationType NotificationType { get; set; }

        public DateTime AddedAtUtc { get; set; }

        public DateTime? SentAtUtc { get; set; }
        public DateTime? ResentAtUtc { get; set; }

        public string Message { get; set; }
        public string Fullname { get; set; }
    }
}
