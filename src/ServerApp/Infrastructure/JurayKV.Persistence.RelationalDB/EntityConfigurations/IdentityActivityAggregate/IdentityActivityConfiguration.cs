using JurayKV.Domain.Aggregates.IdentityActivityAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace JurayKV.Persistence.RelationalDB.EntityConfigurations.IdentityActivityAggregate
{

    public class IdentityActivityConfiguration : IEntityTypeConfiguration<IdentityActivity>
    {
        public void Configure(EntityTypeBuilder<IdentityActivity> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ToTable("IdentityActivitys");
            builder.HasKey(d => d.Id);

            builder.HasOne(emp => emp.User).WithMany().HasForeignKey(emp => emp.UserId).IsRequired();


            builder.Property(d => d.Activity)
         .HasMaxLength(20000) // Adjust the maximum length as needed
         .IsRequired();

            builder.Property(d => d.CreatedAtUtc)
               .IsRequired();
        }
    }
}
