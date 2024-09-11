using JurayKV.Domain.Aggregates.DepartmentAggregate;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JurayKV.Domain.Aggregates.NotificationAggregate;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JurayKV.Persistence.RelationalDB.EntityConfigurations.NotificationAggregate
{

    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.ToTable("Notifications");
            builder.HasKey(d => d.Id);


            builder.HasOne(emp => emp.User).WithMany().HasForeignKey(emp => emp.UserId).IsRequired();

            builder.Property(eu => eu.SentAtUtc).HasColumnType("datetime2");
            builder.Property(eu => eu.AddedAtUtc).HasColumnType("datetime2");
            builder.Property(eu => eu.ResentAtUtc).HasColumnType("datetime2");
            builder.Property(d => d.NotificationStatus);
            builder.Property(d => d.NotificationType);
            builder.Property(d => d.Message);
        }
    }

}
