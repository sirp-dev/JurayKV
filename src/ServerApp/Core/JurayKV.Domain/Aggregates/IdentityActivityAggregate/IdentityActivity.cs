using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.IdentityActivityAggregate
{
    public sealed class IdentityActivity :AggregateRoot
    {
        public IdentityActivity(Guid id)
        {
            Id = id;
            CreatedAtUtc = DateTime.UtcNow;
        }

        // This is needed for EF Core query mapping or deserialization.
        public IdentityActivity()
        {
        }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
                 
        public string Activity { get; set; }

        public DateTime CreatedAtUtc { get; set; }

    }
}
