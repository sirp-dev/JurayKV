using JurayKV.Application.Queries.CompanyQueries;
using JurayKV.Application.Queries.KvPointQueries;
using JurayKV.Application.Queries.TransactionQueries;
using JurayKV.Application.Queries.WalletQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Queries.UserManagerQueries
{
        public class FullUserManagerDetailsDto
    {
        public UserManagerDetailsDto UserManagerDetailsDto { get; set; }
      public  CompanyDetailsDto CompanyDetailsDto { get; set; } 
        public WalletDetailsDto WalletDetailsDto { get; set; }
        public int TotalReferrals { get;set; }
        public List<UserManagerListDto> LastTenReferrals { get; set; }

        public int PointCount { get; set; }
        public decimal PointSum { get; set; }
        public List<KvPointListDto> LastTenPoints { get; set; }

        public int PostingCount { get; set; }
        public int UploadCount { get; set; }

        public int TransactionCreditCount { get; set; }
        public int TransactionDebitCount { get; set; }
        public List<TransactionDescription> GroupByDescriptionCredit { get; set; }
        public List<TransactionDescription> GroupByDescriptionDebit { get; set; }
    }
    public class TransactionDescription
    {
        public string Description { get; set; }
        public List<TransactionListDto> ListTransactions { get; set; }
    }
}
