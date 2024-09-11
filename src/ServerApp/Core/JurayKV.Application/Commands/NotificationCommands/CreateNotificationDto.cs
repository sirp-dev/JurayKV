using JurayKV.Domain.Aggregates.IdentityAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Commands.NotificationCommands
{
    public class CreateNotificationDto
    {
        public Guid UserId { get; set; }
        public NotificationType NotificationType { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
    }
}
