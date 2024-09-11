using JurayKV.Domain.Aggregates.CompanyAggregate;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.WalletAggregate;
using JurayKV.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Domain.Aggregates.AdvertRequestAggregate
{
       public sealed class AdvertRequest : AggregateRoot
    {
        public AdvertRequest(Guid id)
        {
            Id = id;
            CreatedAtUtc = DateTime.UtcNow;
        }

        // This is needed for EF Core query mapping or deserialization.
        public AdvertRequest()
        {
        }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }

        public string ImageUrl { get; set; }
        public string ImageKey { get; set; }

        public string Note { get; set; }
        public decimal Amount { get; set; }

        public DateTime CreatedAtUtc { get; set; }

         public EntityStatus Status { get; set; }
         
        public string TransactionReference { get; set; }
    }

}
