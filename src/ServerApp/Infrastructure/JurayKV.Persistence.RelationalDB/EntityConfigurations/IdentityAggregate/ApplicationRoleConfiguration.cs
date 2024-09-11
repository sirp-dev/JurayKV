using JurayKV.Domain.Aggregates.IdentityAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JurayKV.Persistence.RelationalDB.EntityConfigurations.IdentityAggregate;

internal sealed class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        ////builder.Property<int>("IdentityKey").ValueGeneratedOnAdd();
    }
}
