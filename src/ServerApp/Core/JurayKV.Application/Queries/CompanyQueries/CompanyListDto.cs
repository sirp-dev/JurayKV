using JurayKV.Domain.Aggregates.IdentityAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Queries.CompanyQueries
{
    public class CompanyListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public decimal AmountPerPoint { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
