using JurayKV.Domain.Aggregates.CompanyAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.RelationalDB.EntityConfigurations.CompanyAggregate
{

    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ToTable("Companys");
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Name)
          .HasMaxLength(255) // Adjust the maximum length as needed
          .IsRequired();

            builder.Property(d => d.CreatedAtUtc)
                .IsRequired();
        }
    }
}
