using JurayKV.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Domain.Aggregates.CategoryVariationAggregate
{
   
    public sealed class CategoryVariation : AggregateRoot
    {
        public CategoryVariation(Guid id)
        {
            Id = id;
             
        }

        // This is needed for EF Core query mapping or deserialization.
        public CategoryVariation()
        {
        }

        public string Name { get; set; }
        public string Code { get; set; }
        public VariationType VariationType { get; set; }
        public bool Active { get; set; }
        public decimal Charge { get; set; }
        public BillGateway BillGateway { get; set; }
        public Tier Tier { get; set; }

        public string? Url { get; set; }
        public string? Key { get; set; }
    }
}
