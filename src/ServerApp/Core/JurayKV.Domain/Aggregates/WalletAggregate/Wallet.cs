using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Domain.Aggregates.WalletAggregate
{
    public sealed class Wallet : AggregateRoot
    {
        public Wallet(Guid id)
        {
            Id = id;
            CreatedAtUtc = DateTime.UtcNow;
            LastUpdateAtUtc = DateTime.UtcNow;
        }

        // This is needed for EF Core query mapping or deserialization.
        public Wallet()
        {
        }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Note { get; set; }
        public string Log { get; set; }
        public decimal Amount { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        public DateTime LastUpdateAtUtc { get; set; }

    }
}
