using JurayKV.Domain.Aggregates.BucketAggregate;
using JurayKV.Domain.Aggregates.CompanyAggregate;
using JurayKV.Domain.Aggregates.TransactionAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JurayKV.Domain.Aggregates.WalletAggregate;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Persistence.RelationalDB.EntityConfigurations.TransactionAggregate
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ToTable("Transactions");
            builder.HasKey(d => d.Id);

            builder.HasOne(emp => emp.User).WithMany().HasForeignKey(emp => emp.UserId).IsRequired();


            builder.HasOne<Wallet>(emp => emp.Wallet)
    .WithOne()
    .HasForeignKey<Transaction>(emp => emp.WalletId)
    .IsRequired();

            builder.Property(d => d.Note)
                   .HasMaxLength(20000);
            builder.Property(d => d.Amount)
             .HasColumnType("decimal(18, 2)") // Adjust the data type and precision/scale as needed
             .IsRequired();
            builder.Property(d => d.CreatedAtUtc)
     .IsRequired();

            builder.Property(d => d.TransactionType)
       .IsRequired()
       .HasConversion(
           v => v.ToString(),
           v => (TransactionTypeEnum)Enum.Parse(typeof(TransactionTypeEnum), v)
       );

            builder.Property(d => d.Status)
.IsRequired()
.HasConversion(
   v => v.ToString(),
   v => (EntityStatus)Enum.Parse(typeof(EntityStatus), v)
);
            builder.Property(d => d.TransactionReference)
                 .HasMaxLength(200);

            builder.Property(d => d.Description)
                 .HasMaxLength(200);
            builder.Property(d => d.TrackCode)
                 .HasMaxLength(50);

        }
    }

}
