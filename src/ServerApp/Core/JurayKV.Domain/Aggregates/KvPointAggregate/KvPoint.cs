using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.IdentityKvAdAggregate;
using JurayKV.Domain.Primitives;
using System;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Domain.Aggregates.KvPointAggregate
{
    public sealed class KvPoint : AggregateRoot
    {
        public KvPoint(Guid id)
        {
            Id = id;
            CreatedAtUtc = DateTime.UtcNow;
        }

        // This is needed for EF Core query mapping or deserialization.
        public KvPoint()
        {
        }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid IdentityKvAdId { get; set; }
        public IdentityKvAd IdentityKvAd { get; private set; }
        public EntityStatus Status { get; set; }

        public int Point { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        public DateTime? LastModifiedAtUtc { get; set; }
        public string PointHash { get; set; }


    }
}
