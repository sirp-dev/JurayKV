using JurayKV.Domain.Aggregates.CompanyAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Queries.AdvertRequestQueries
{
    public class AdvertRequestDetailsListDto
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }

        public string ImageUrl { get; set; }
        public string ImageKey { get; set; }

        public string Note { get; set; }
        public decimal Amount { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public EntityStatus Status { get; set; }

        public Guid TransactionReference { get; set; }
    }
}
