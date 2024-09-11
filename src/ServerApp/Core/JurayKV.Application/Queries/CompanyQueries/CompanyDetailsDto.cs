using JurayKV.Domain.Aggregates.IdentityAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Queries.CompanyQueries
{
    public class CompanyDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public decimal AmountPerPoint { get; set; }

    }
}
