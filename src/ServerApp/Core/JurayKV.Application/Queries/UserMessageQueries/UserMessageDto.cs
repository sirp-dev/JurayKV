using JurayKV.Domain.Aggregates.IdentityAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Queries.UserMessageQueries
{
    public class UserMessageDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string? Message { get; set; }
        public string? Title { get; set; }
        public string? FileUrl { get; set; }
        public string? FileKey { get; set; }
        public bool Read { get; set; }
        public DateTime? DateRead { get; set; }

        public Guid? UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string? AllUserIdRead { get; set; }

        public bool Disable { get; set; }
        public bool All { get; set; }
    }
}
