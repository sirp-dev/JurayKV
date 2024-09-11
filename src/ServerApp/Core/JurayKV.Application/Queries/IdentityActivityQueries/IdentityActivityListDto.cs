using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Queries.IdentityActivityQueries
{
    public class IdentityActivityListDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Fullname { get; set; }
        public string Activity { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }
}
