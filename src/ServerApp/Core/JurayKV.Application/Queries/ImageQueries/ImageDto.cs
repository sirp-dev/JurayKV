using JurayKV.Domain.Aggregates.KvAdAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Queries.ImageQueries
{
    public class ImageDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string ImageKey { get; set; }
        public bool ShowInDropdown { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        public KvAd KvAd { get; set; }
    }
}
