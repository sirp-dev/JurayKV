using JurayKV.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.StateLgaAggregate
{
    public class State : AggregateRoot
    {
 

        public string StateName { get; set; }

        public virtual ICollection<LocalGoverment> LocalGov { get; set; }


    }
}
