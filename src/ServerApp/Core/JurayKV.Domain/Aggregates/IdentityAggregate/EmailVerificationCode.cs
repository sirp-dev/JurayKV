using JurayKV.Domain.Aggregates.DepartmentAggregate;
using JurayKV.Domain.Exceptions;
using JurayKV.Domain.ValueObjects;
using System;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.IdentityAggregate;

public class EmailVerificationCode
{
    public Guid Id { get; set; }

    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string UserId { get; set; }
    public string Code { get; set; }

    public DateTime SentAtUtc { get; set; }

    public DateTime? UsedAtUtc { get; set; }

     
}
