using JurayKV.Domain.Aggregates.CategoryVariationAggregate;
using JurayKV.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Domain.Aggregates.VariationAggregate
{
      public sealed class Variation : AggregateRoot
    {
        public Variation(Guid id)
        {
            Id = id;
             
        }

        // This is needed for EF Core query mapping or deserialization.
        public Variation()
        {
        }

        public string Name { get; set; }
        public string Code { get; set; }
        public string Amount { get; set; }
        public bool Active { get; set; }

        public string? Url { get; set; }
        public string? Key { get; set; }
        public BillGateway BillGateway { get; set; }
        public Tier Tier { get; set; }
        public Guid CategoryVariationId { get; set; }
        public CategoryVariation CategoryVariation { get; set; }

        
    }
}
