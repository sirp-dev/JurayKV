using System;
using Microsoft.AspNetCore.Identity;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Domain.Aggregates.IdentityAggregate;

public class ApplicationUser : IdentityUser<Guid>
{
    public string SurName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IdNumber {  get; set; }
    public AccountStatus AccountStatus { get; set; }

    public DateTime? LastLoggedInAtUtc { get; set; }
    public DateTime CreationUTC { get; set; }
    
    public RefreshToken RefreshToken { get; set; }

    public string Xvalue { get; set; }
    public string XtxtGuid { get; set; }
    public DateTime XvalueDate { get; set; }

    public bool DisableEmailNotification { get; set; }

    public string RefferedByPhoneNumber { get;set; }
    public Tier Tier {  get; set; }
    public DateTime DateUpgraded { get; set; }
    public string? About { get; set; }
    public string? AlternativePhone { get; set; }
    public string? Address { get; set; }
    public string? State { get; set; }
    public string? LGA { get; set; }

    public DateTime? DateOfBirth { get; set; }
    public string? StateOfOrigin { get; set; }
    public string? LGA_Of_Origin { get; set; }
    public string? NinNumber { get; set; }

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
    //public DateTime DateTie2Upgraded { get; set; }
    public string? ResponseOnCsaRequest { get;set; }
    public string? ResponseOnTieRequest { get;set; }
    public bool CsaRequest { get;set; }
    public TieRequestStatus Tie2Request { get; set; }

    public string? VerificationCode { get; set; }
    public string? Role { get; set; }






    public bool Posted { get;set; }
    public bool VideoUpload { get; set; }
    public bool SuccessPoint { get; set; }





}
