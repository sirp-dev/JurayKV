using JurayKV.Domain.Aggregates.KvAdAggregate;
using JurayKV.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.ImageAggregate
{
   
        public sealed class ImageFile : AggregateRoot
        {
            public ImageFile(Guid id)
            {
                Id = id;
                CreatedAtUtc = DateTime.UtcNow;
            }

            // This is needed for EF Core query mapping or deserialization.
            public ImageFile()
            {
            }
        public string Name { get; set; }

        public string ImageUrl { get; set; }
        public string ImageKey { get; set; }
        public bool ShowInDropdown { get; set; }
        public DateTime CreatedAtUtc { get; set; }

        public List<KvAd> KvAds { get; set; } = new List<KvAd>();
     }
}
