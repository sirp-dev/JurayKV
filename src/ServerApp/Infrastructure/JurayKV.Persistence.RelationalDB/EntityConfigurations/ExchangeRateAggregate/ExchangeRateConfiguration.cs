using JurayKV.Domain.Aggregates.ExchangeRateAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.RelationalDB.EntityConfigurations.ExchangeRateAggregate
{
     public class ExchangeRateConfiguration : IEntityTypeConfiguration<ExchangeRate>
    {
        public void Configure(EntityTypeBuilder<ExchangeRate> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ToTable("ExchangeRates");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Amount)
              .HasColumnType("decimal(18, 2)") // Adjust the data type and precision/scale as needed
              .IsRequired();

            builder.Property(d => d.CreatedAtUtc)
               .IsRequired();
        }
    }

}
