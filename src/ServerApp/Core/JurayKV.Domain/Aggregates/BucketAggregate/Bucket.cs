using JurayKV.Domain.Aggregates.DepartmentAggregate;
using JurayKV.Domain.Aggregates.KvAdAggregate;
using JurayKV.Domain.Primitives;
using JurayKV.Domain.ValueObjects;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.BucketAggregate
{
    public sealed class Bucket : AggregateRoot
    {
        public Bucket(Guid id)
        {
            Id = id;
            AdminActive = true;
            Disable = false;
            UserActive = true;
            CreatedAtUtc = DateTime.UtcNow;
        }

        // This is needed for EF Core query mapping or deserialization.
        public Bucket()
        {
        }

        public string Name { get; set; }

        public bool Disable { get; set; }

        public bool AdminActive { get; set; }

        public bool UserActive { get; set; }

        public DateTime CreatedAtUtc { get; set; } 
    }
}
