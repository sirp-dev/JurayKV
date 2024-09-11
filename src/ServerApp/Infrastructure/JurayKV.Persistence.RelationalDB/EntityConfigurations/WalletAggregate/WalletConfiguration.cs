using JurayKV.Domain.Aggregates.WalletAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.RelationalDB.EntityConfigurations.WalletAggregate
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ToTable("Wallets");
            builder.HasKey(d => d.Id);

            
            builder.HasOne(emp => emp.User).WithMany().HasForeignKey(emp => emp.UserId).IsRequired();


            builder.Property(d => d.Note)
         .HasMaxLength(200) // Adjust the maximum length as needed
         .IsRequired();
            builder.Property(d => d.Log)
   .HasMaxLength(2000000000) // Adjust the maximum length as needed
   .IsRequired();
            builder.Property(d => d.CreatedAtUtc)
               .IsRequired();
            builder.Property(d => d.LastUpdateAtUtc);
        }
    }
}
