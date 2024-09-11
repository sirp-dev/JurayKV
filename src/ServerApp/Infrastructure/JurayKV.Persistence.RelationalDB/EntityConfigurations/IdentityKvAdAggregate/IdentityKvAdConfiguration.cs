using JurayKV.Domain.Aggregates.IdentityKvAdAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JurayKV.Domain.Aggregates.KvAdAggregate;

namespace JurayKV.Persistence.RelationalDB.EntityConfigurations.IdentityKvAdAggregate
{
    public class IdentityKvAdConfiguration : IEntityTypeConfiguration<IdentityKvAd>
    {
        public void Configure(EntityTypeBuilder<IdentityKvAd> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ToTable("IdentityKvAds");
            builder.HasKey(d => d.Id);

  
            builder.HasOne(emp => emp.User).WithMany().HasForeignKey(emp => emp.UserId).IsRequired();
            builder.HasOne(emp => emp.KvAd).WithMany().HasForeignKey(emp => emp.KvAdId).IsRequired();
 
            builder.Property(d => d.CreatedAtUtc)
     .IsRequired();
            builder.Property(d => d.LastModifiedAtUtc)
            .IsRequired(false);
            builder.Property(d => d.VideoKey)
        .HasMaxLength(20);

            builder.Property(d => d.VideoUrl)
      .HasMaxLength(500);

            builder.Property(d => d.KvAdHash)
      .HasMaxLength(50);

            builder.Property(d => d.ResultOne)
        .IsRequired().HasDefaultValue(0);
            builder.Property(d => d.ResultTwo)
        .IsRequired().HasDefaultValue(0);
            builder.Property(d => d.ResultThree)
        .IsRequired().HasDefaultValue(0);

            builder.Property(d => d.Active)
               .IsRequired();
        }
    }
}
