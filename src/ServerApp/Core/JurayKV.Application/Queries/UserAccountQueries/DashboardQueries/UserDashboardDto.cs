using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Queries.UserAccountQueries.DashboardQueries
{
    public class UserDashboardDto
    {
        public string Fullname { get;set; }
        public string Company { get;set; }
        public string Passport { get;set; }
        public string Status { get; set; }
        public decimal Points { get;set;}
        public decimal Balance { get;set; }
        public int TotalViews { get;set; }
        public int AdsRunning { get;set; }
        public decimal ExchangeRate { get;set; }
     
        public ICollection<WeeklyPointHistory> WeeklyPointHistories { get;set; }
        public ICollection<LastTenPoints> LastTenPoints { get;set; }
        public ICollection<LastTenTransactions> LastTenTransactions { get;set; }
        public ICollection<ListRunningAds> ListRunningAds { get;set; }
        public ICollection<LastTenAds> LastTenAds { get;set; }
        public int TransactionsCount { get;set; }
        public int AdsCount { get;set; }
        public bool Upgrade {  get;set; }

    }
    public class ListRunningAds
    {
        public Guid AdsId { get; set; }
        public string Image { get; set; }
    }
    public class WeeklyPointHistory
    {
        public int Point {  get;set; }
        public string Day {  get;set; }
    }

    public class LastTenPoints
    {
        public Guid Id { get;set; }
        public EntityStatus Status { get; set; }

        public int Point { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
    public class LastTenAds
    {
        public Guid Id { get; set; } 
        public string Image { get; set; }
        public string ImageKey { get; set; }

        public int Views { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }

    public class LastTenTransactions
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public TransactionTypeEnum TransactionType { get; set; }
        public EntityStatus Status { get; set; }
        public string Note {  get; set; }
    }
}
