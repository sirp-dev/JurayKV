using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.IdentityKvAdAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Queries.KvPointQueries
{
    public class KvPointListDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Fullname { get; set; }
        public Guid IdentityKvAdId { get; set; }
        public EntityStatus Status { get; set; }

        public int Point { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        public DateTime? LastModifiedAtUtc { get; set; }
        public string PointHash { get; set; }
    }
}
