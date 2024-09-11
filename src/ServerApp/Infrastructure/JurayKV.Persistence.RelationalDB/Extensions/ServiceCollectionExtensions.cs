using JurayKV.Domain.Aggregates.AdvertRequestAggregate;
using JurayKV.Domain.Aggregates.BucketAggregate;
using JurayKV.Domain.Aggregates.CategoryVariationAggregate;
using JurayKV.Domain.Aggregates.CompanyAggregate;
using JurayKV.Domain.Aggregates.DashboardAggregate;
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
using JurayKV.Persistence.RelationalDB.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Persistence.RelationalDB.Extensions;

public static class ServiceCollectionExtensions
{
    public static readonly ILoggerFactory MyLoggerFactory
        = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name
                    && level == LogLevel.Information)
                .AddConsole();
        });

    public static void AddRelationalDbContext(
        this IServiceCollection services,
        string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Connection string is either null or empty.");
        }

        services.AddDbContext<JurayDbContext>(options =>
        {
            options.UseLoggerFactory(MyLoggerFactory);
            options.EnableSensitiveDataLogging(true);
            //options.UseSqlServer(connectionString, builder =>
            //{
            //    ////builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);
            //    //builder.MigrationsAssembly("JurayKV.Persistence.RelationalDB");
            //    //builder.MigrationsHistoryTable("__EFCoreMigrationsHistory", schema: "_Migration");
            //});
            options.UseSqlServer(connectionString);
        });

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.@";
            options.User.RequireUniqueEmail = true;
          
            options.SignIn.RequireConfirmedEmail = true;
         })
        .AddEntityFrameworkStores<JurayDbContext>()
        .AddDefaultTokenProviders();

        services.AddTransient<IDepartmentRepository, DepartmentRepository>();
        services.AddTransient<EmployeeFactory>();
        services.AddTransient<IEmployeeRepository, EmployeeRepository>();
        services.AddTransient<INotificationRepository, NotificationRepository>();
        services.AddScoped<IBucketRepository, BucketRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
        services.AddScoped<IIdentityActivityRepository, IdentityActivityRepository>();
        services.AddScoped<IIdentityKvAdRepository, IdentityKvAdRepository>();
        services.AddScoped<IKvAdRepository, KvAdRepository>();
        services.AddScoped<IKvPointRepository, KvPointRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<IDashboardRepository, DashboardRepository>();
        services.AddScoped<ISliderRepository, SliderRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<ISettingRepository, SettingRepository>();
        services.AddScoped<IAdvertRequestRepository, AdvertRequestRepository>();
        services.AddScoped<IVariationRepository, VariationRepository>();
        services.AddScoped<ICategoryVariationRepository, CategoryVariationRepository>();
        //services.AddScoped<IUserMessageRepository, UserMessageRepository>();
        

        services.AddScoped<IStateRepository, StateRepository>();
        services.AddScoped<ILgaRepository, LgaRepository>();
        services.AddScoped<IUserMessageRepository, UserMessageRepository>();


        services.AddGenericRepository<JurayDbContext>();
        services.AddQueryRepository<JurayDbContext>();
    }

    public static void AddWebApiRelationalDbContext(
        this IServiceCollection services,
        string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Connection string is either null or empty.");
        }

        services.AddDbContext<JurayDbContext>(options =>
        {
          
            options.UseSqlServer(connectionString);
        });

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
           
        })
        .AddEntityFrameworkStores<JurayDbContext>()
        .AddDefaultTokenProviders();


    }

}
