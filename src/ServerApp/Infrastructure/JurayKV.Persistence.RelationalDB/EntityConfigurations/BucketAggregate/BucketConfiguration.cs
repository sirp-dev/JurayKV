using JurayKV.Domain.Aggregates.BucketAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.RelationalDB.EntityConfigurations.BucketAggregate
{
    public class BucketConfiguration : IEntityTypeConfiguration<Bucket>
    {
        public void Configure(EntityTypeBuilder<Bucket> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ToTable("Buckets");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
           .HasMaxLength(255) // Adjust the maximum length as needed
           .IsRequired();

            builder.Property(d => d.Disable)
                .IsRequired();

            builder.Property(d => d.AdminActive)
                .IsRequired();

            builder.Property(d => d.UserActive)
                .IsRequired();

            builder.Property(d => d.CreatedAtUtc)
                .IsRequired();

        }
    }
}
