using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Queries.UserManagerQueries
{
    public class UserManagerDetailsDto
    {
        public Guid Id { get; set; }
        public string Fullname { get; set; }
        public string IdNumber { get; set; }

        public string Surname { get; set; } 
        public string Firstname { get; set; } 
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public AccountStatus AccountStatus { get; set; }

        public DateTime CreationUTC { get; set; }

        public DateTime? LastLoggedInAtUtc { get; set; }
        public string? NinNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string? StateOfOrigin { get; set; }
        public string? LGA_Of_Origin { get; set; }
        public decimal WalletBalance { get;set; }
        public bool DisableEmailNotification { get; set; }
        public bool IsCompany {  get; set; }
        public string RefferedBy { get; set; }
        public string PhoneOfRefferedBy { get; set; }
        public bool IsCSARole { get; set; }
        public string? Role { get;set;}
        public Tier Tier { get; set; }
        public DateTime DateUpgraded { get; set; }
        public string? About { get; set; }
        public string? AlternativePhone { get; set; }
        public string? Address { get; set; }
        public string? State { get; set; }
        public string? LGA { get; set; }
        public string? Occupation { get; set; }
        public string? FbHandle { get; set; }
        public string? InstagramHandle { get; set; }
        public string? TwitterHandle { get; set; }
        public string? TiktokHandle { get; set; }
        public string? IDCardUrl { get; set; }
        public string? IDCardKey { get; set; }
        public string? PassportUrl { get; set; }
        public string? PassportKey { get; set; }

        public string? AccountNumber { get; set; }
        public string? AccountName { get; set; }
        public string? BankName { get; set; }
        public string? BVN { get; set; }
        public DateTime? RequestDateTie2Upgraded { get; set; }
        public string? ResponseOnCsaRequest { get; set; }
        public bool CsaRequest { get; set; }
         
        public string? ResponseOnTieRequest { get; set; } 
        public TieRequestStatus Tie2Request { get; set; }

        public bool EmailComfirmed { get; set; }
        public bool TwoFactorEnable { get; set; }

        public string RefCode { get; set; }
    }
}
