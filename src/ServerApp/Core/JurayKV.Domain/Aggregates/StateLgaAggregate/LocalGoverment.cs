using JurayKV.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.StateLgaAggregate
{
    public class LocalGoverment : AggregateRoot
    {
         public string LGAName { get; set; }

        public Guid StatesId { get; set; }
        public State States { get; set; }
    }
}
