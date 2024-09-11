using JurayKV.Domain.Aggregates.AdvertRequestAggregate;
using JurayKV.Domain.Aggregates.BucketAggregate;
using JurayKV.Domain.Aggregates.CategoryVariationAggregate;
using JurayKV.Domain.Aggregates.CompanyAggregate;
using JurayKV.Domain.Aggregates.DepartmentAggregate;
using JurayKV.Domain.Aggregates.EmployeeAggregate;
using JurayKV.Domain.Aggregates.ExchangeRateAggregate;
using JurayKV.Domain.Aggregates.IdentityActivityAggregate;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.IdentityKvAdAggregate;
using JurayKV.Domain.Aggregates.ImageAggregate;
using JurayKV.Domain.Aggregates.KvAdAggregate;
using JurayKV.Domain.Aggregates.KvPointAggregate;
using JurayKV.Domain.Aggregates.NotificationAggregate;
using JurayKV.Domain.Aggregates.SettingAggregate;
using JurayKV.Domain.Aggregates.SliderAggregate;
using JurayKV.Domain.Aggregates.StateLgaAggregate;
using JurayKV.Domain.Aggregates.TransactionAggregate;
using JurayKV.Domain.Aggregates.UserMessageAggregate;
using JurayKV.Domain.Aggregates.VariationAggregate;
using JurayKV.Domain.Aggregates.WalletAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JurayKV.Persistence.RelationalDB;

public sealed class JurayDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public JurayDbContext(DbContextOptions<JurayDbContext> options)
        : base(options)
    {
    }
    public DbSet<Bucket> Buckets { get;set; } 
    public DbSet<Company> Companies { get;set; } 
    public DbSet<Department> Departments { get;set; } 
    public DbSet<Employee> Employees { get;set; } 
    public DbSet<ExchangeRate> ExchangeRates { get;set; } 
    public DbSet<IdentityActivity> IdentityActivities { get;set; } 
    public DbSet<IdentityKvAd> IdentityKvAds { get;set; } 
    public DbSet<KvAd> kvAds { get;set; }
    public DbSet<KvPoint> kvPoints { get;set; }
    public DbSet<Notification> Notifications { get;set; }
    public DbSet<Transaction> Transactions { get;set; }
    public DbSet<Wallet> Wallets { get;set; }
    public DbSet<EmailVerificationCode> EmailVerificationCodes { get;set; }
    public DbSet<PasswordResetCode> PasswordResetCodes { get;set; }
    public DbSet<UserData> UserDatas { get;set; }
    public DbSet<UserOldPassword> UserOldPassword { get;set; }
    public DbSet<Slider> Sliders { get;set; }
    public DbSet<ImageFile> ImageFiles { get;set; }
    public DbSet<Setting> Settings { get;set; }
    public DbSet<AdvertRequest> AdvertRequests { get;set; }
    public DbSet<Variation> Variations { get;set; }
    public DbSet<CategoryVariation> CategoryVariations { get;set; }


    public DbSet<State> States { get;set; }
    public DbSet<LocalGoverment> LocalGoverments { get;set; }
    public DbSet<UserMessage> UserMessages { get;set; }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ////ChangeTracker.ApplyValueGenerationOnUpdate();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        ////ChangeTracker.ApplyValueGenerationOnUpdate();
        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        //modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
        //modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
        //modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        base.OnModelCreating(modelBuilder);
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmployeeConfiguration).Assembly);
        ////modelBuilder.ApplyBaseEntityConfiguration(); // This should be called after calling the derived entity configurations
    }
}
