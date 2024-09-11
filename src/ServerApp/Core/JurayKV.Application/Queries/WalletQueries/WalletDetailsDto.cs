using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Queries.WalletQueries
{
    public class WalletDetailsDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Fullname { get; set; }
        public string Note { get; set; }
        public string Log { get; set; }
        public decimal Amount { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        public DateTime LastUpdateAtUtc { get; set; }
    }
}
