using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Queries.UserManagerQueries
{
    public class UserManagerListDto
    {
        public Guid Id { get; set; }
        public string IdNumber { get; set; }
        public string Fullname { get; set; }
        public string Surname {  get; set; }
        public string Firstname {  get; set; }
        public string Lastname {  get; set; }
        public string Email { get;set; }
        public string PhoneNumber { get;set; }
        public AccountStatus AccountStatus { get; set; }
        public string? Role { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreationUTC { get; set; }
        public bool Verified { get; set; }
        public DateTime? LastLoggedInAtUtc { get; set; }
        public string? VerificationCode { get; set; }
        public int ReferralCount { get; set; }
        public decimal WalletBalance { get; set; }
         
        public decimal TotalTransactionCredit { get;set; }
        public decimal TotalTransactionDebit { get;set; }
        public decimal TotalPoints { get;set; }
        public decimal TotalReferralAmount { get;set; }

        public Tier Tier { get; set; }
        public DateTime? RequestDateTie2Upgraded { get; set; }
        public TieRequestStatus Tie2Request { get; set; } 


        public bool Posted { get; set; }
        public bool VideoUpload { get; set; }
        public bool SuccessPoint { get; set; }
    }
}
