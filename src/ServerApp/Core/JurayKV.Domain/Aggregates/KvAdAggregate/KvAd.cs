using JurayKV.Domain.Aggregates.BucketAggregate;
using JurayKV.Domain.Aggregates.CompanyAggregate;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.IdentityKvAdAggregate;
using JurayKV.Domain.Aggregates.ImageAggregate;
using JurayKV.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Domain.Aggregates.KvAdAggregate
{
    public sealed class KvAd : AggregateRoot
    {
        public KvAd(Guid id)
        {
            Id = id;
            CreatedAtUtc = DateTime.UtcNow;
        }

        // This is needed for EF Core query mapping or deserialization.
        public KvAd()
        {
        }

        public Guid UserId { get; set; }
        public ApplicationUser User { get;set; }
        public Guid BucketId { get; set; }
        public Bucket Bucket { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public Guid? ImageFileId { get; set; }
        public ImageFile ImageFile { get; set; }
        public DataStatus Status { get;set; }
        public bool Active { get; set; }
        public string DateId { get; set; }

        public ICollection<IdentityKvAd> IdentityKvAds { get; set; }
    }
}
