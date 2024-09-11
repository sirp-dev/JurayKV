using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JurayKV.Domain.Aggregates.IdentityAggregate;

[NotMapped]
public class RefreshToken
{
    public Guid UserId { get; set; }

    public string Token { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime ExpireAtUtc { get; set; }

    // Navigation properties
    //public ApplicationUser ApplicationUser { get; set; }
}
