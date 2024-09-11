using JurayKV.Domain.Aggregates.KvAdAggregate;
using JurayKV.Domain.Aggregates.KvAdAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JurayKV.Domain.Aggregates.BucketAggregate;
using JurayKV.Domain.Aggregates.CompanyAggregate;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Persistence.RelationalDB.EntityConfigurations.KvAdAggregate
{

     public class KvAdConfiguration : IEntityTypeConfiguration<KvAd>
    {
        public void Configure(EntityTypeBuilder<KvAd> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ToTable("KvAds");
            builder.HasKey(d => d.Id);

            
            builder.HasOne(emp => emp.User).WithMany().HasForeignKey(emp => emp.UserId).IsRequired();
            builder.HasOne(emp => emp.Bucket).WithMany().HasForeignKey(emp => emp.BucketId).IsRequired();
            builder.HasOne(emp => emp.Company).WithMany().HasForeignKey(emp => emp.CompanyId).IsRequired();
             

            builder.Property(d => d.CreatedAtUtc)
     .IsRequired();

            builder.Property(d => d.ImageKey)
        .HasMaxLength(200);

            builder.Property(d => d.ImageUrl)
      .HasMaxLength(500);

            builder.Property(d => d.Status)
.IsRequired()
.HasConversion(
  v => v.ToString(),
  v => (DataStatus)Enum.Parse(typeof(DataStatus), v)
);
        }
    }
}
