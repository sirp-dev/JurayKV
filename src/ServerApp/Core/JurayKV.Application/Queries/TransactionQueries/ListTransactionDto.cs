using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Queries.TransactionQueries
{
    public class ListTransactionDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone {  get; set; }
        public decimal TotalPoints { get; set; }
        public decimal TotalReferrals { get; set; }
        public decimal TotalDebit {  get; set; }
        public decimal WalletBalance { get; set; }

        public int TotalInList { get; set; }
         
    }
}
