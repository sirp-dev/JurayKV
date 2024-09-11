using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.KvAdAggregate;
using JurayKV.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Domain.Aggregates.IdentityKvAdAggregate
{
    public sealed class IdentityKvAd : AggregateRoot
    {
        public IdentityKvAd(Guid id)
        {
            Id = id;
            CreatedAtUtc = DateTime.UtcNow.AddHours(1);
        }

        // This is needed for EF Core query mapping or deserialization.
        public IdentityKvAd()
        {
        }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; private set; }
        public Guid KvAdId { get;  set; }
        public KvAd KvAd { get; private set; }
        public bool Active { get; set; }
        public string Activity { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        public DateTime? LastModifiedAtUtc { get; set; }
        public string? VideoUrl { get; set; }
        public string? VideoKey { get; set; }

        public AdsStatus AdsStatus { get;set; }

        public string KvAdHash { get; set; }
        public int ResultOne { get; set; }
        public int ResultTwo { get; set; }
        public int ResultThree { get; set; }

    }
}
