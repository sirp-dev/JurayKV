using JurayKV.Domain.Aggregates.KvPointAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Persistence.RelationalDB.EntityConfigurations.KvPointAggregate
{
    public class KvPointConfiguration : IEntityTypeConfiguration<KvPoint>
    {
        public void Configure(EntityTypeBuilder<KvPoint> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ToTable("KvPoints");
            builder.HasKey(d => d.Id);

            builder.HasOne(emp => emp.User).WithMany().HasForeignKey(emp => emp.UserId).IsRequired();
            builder.HasOne(emp => emp.IdentityKvAd).WithMany().HasForeignKey(emp => emp.IdentityKvAdId).IsRequired();

            builder.Property(d => d.CreatedAtUtc)
     .IsRequired();
            builder.Property(d => d.LastModifiedAtUtc)
            .IsRequired(false);
            builder.Property(d => d.Status)
      .IsRequired()
      .HasConversion(
          v => v.ToString(),
          v => (EntityStatus)Enum.Parse(typeof(EntityStatus), v));

            builder.Property(d => d.PointHash)
      .HasMaxLength(500);
        }
    }
}
