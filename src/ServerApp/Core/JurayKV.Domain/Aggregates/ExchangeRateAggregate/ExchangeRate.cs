using JurayKV.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.ExchangeRateAggregate
{
    public sealed class ExchangeRate : AggregateRoot
    {
        public ExchangeRate(Guid id)
        {
            Id = id;
            
            CreatedAtUtc = DateTime.UtcNow;
        }

        // This is needed for EF Core query mapping or deserialization.
        public ExchangeRate()
        {
        }

        public decimal Amount { get; set; }

        public DateTime CreatedAtUtc { get; set; }

    }
}
