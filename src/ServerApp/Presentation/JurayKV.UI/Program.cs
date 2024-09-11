using JurayKV.Application;
using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Commands.DepartmentCommands;
using JurayKV.Application.Infrastructures;
using JurayKV.Application.Services;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.StateLgaAggregate;
using JurayKV.Domain.Aggregates.VariationAggregate;
using JurayKV.Infrastructure.Flutterwave.Repositories;
using JurayKV.Infrastructure.Opay.core.Repositories;
using JurayKV.Infrastructure.Services;
using JurayKV.Infrastructure.VTU.Repository;
using JurayKV.Persistence.Cache;
using JurayKV.Persistence.Cache.Repositories;
using JurayKV.Persistence.RelationalDB;
using JurayKV.Persistence.RelationalDB.Extensions;
using JurayKV.UI.Jobs;
using JurayKV.UI.Services;
using JurayKvV.Infrastructure.Interswitch.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PostmarkEmailService;
using Serilog;
using System.Reflection;

namespace JurayKV.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
       
            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


            builder.Services.AddRazorPages();

            builder.Services.AddCaching();
            string sendGridApiKey = "yourSendGridKey";
            builder.Services.AddSendGrid(sendGridApiKey);
            builder.Services.AddRelationalDbContext(connectionString);
             

            builder.Services.AddHostedService<BackgroundTask>();
            builder.Services.AddScoped<BackgroundActivity>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateDepartmentCommand>());
            builder.Services.AddTransient<IEmployeeCacheRepository, EmployeeCacheRepository>();
            builder.Services.AddTransient<IDepartmentCacheRepository, DepartmentCacheRepository>();
            builder.Services.AddTransient<INotificationCacheRepository, NotificationCacheRepository>();

            builder.Services.AddScoped<IBucketCacheRepository, BucketCacheRepository>();
            builder.Services.AddScoped<ICompanyCacheRepository, CompanyCacheRepository>();
            builder.Services.AddScoped<IExchangeRateCacheRepository, ExchangeRateCacheRepository>();
            builder.Services.AddScoped<IIdentityActivityCacheRepository, IdentityActivityCacheRepository>();
            builder.Services.AddScoped<IIdentityKvAdCacheRepository, IdentityKvAdCacheRepository>();

            builder.Services.AddScoped<IKvAdCacheRepository, KvAdCacheRepository>();
            builder.Services.AddScoped<IKvPointCacheRepository, KvPointCacheRepository>();
            builder.Services.AddScoped<ITransactionCacheRepository, TransactionCacheRepository>();
            builder.Services.AddScoped<IWalletCacheRepository, WalletCacheRepository>();
            builder.Services.AddScoped<IDashboardCacheRepository, DashboardCacheRepository>();
            builder.Services.AddScoped<IUserManagerCacheRepository, UserManagerCacheRepository>();
            builder.Services.AddScoped<ISliderCacheRepository, SliderCacheRepository>();
            builder.Services.AddScoped<IImageCacheRepository, ImageCacheRepository>();
            builder.Services.AddScoped<ISettingCacheRepository, SettingCacheRepository>();
            builder.Services.AddScoped<IAdvertRequestCacheRepository, AdvertRequestCacheRepository>(); 

            builder.Services.AddTransient<IExceptionLogger, ExceptionLogger>();
            builder.Services.AddTransient<ViewRenderService>();
            //builder.Services.AddTransient<SendGridConfig>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddTransient<ISmsSender, SmsSender>();
            builder.Services.AddTransient<IWhatsappOtp, WhatsappOtp>();
            builder.Services.AddTransient<IVoiceSender, VoiceSender>();
            builder.Services.AddTransient<IStorageService, StorageService>();
            builder.Services.AddTransient<IFlutterTransactionService, FlutterTransactionService>();
            builder.Services.AddTransient<ISwitchRepository, SwitchRepository>();
            builder.Services.AddTransient<IBackgroundActivity, BackgroundActivity>();
            builder.Services.AddTransient<IOpayRepository, OpayRepository>();
            builder.Services.AddTransient<IVtuService, VtuService>();
           
            // IOpayRepository
            //builder.Services.AddInterswitch(configuration);
            //builder.Services.Configure<RaveConfig>(Configuration.GetSection("RaveConfig"));
            // Access the configuration using builder.Configuration
            var raveConfigSection = builder.Configuration.GetSection("RaveConfig");
             

            // Add services to the container.
            builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));


            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(Constants.SuperAdminPolicy, policy =>
                {
                    policy.RequireRole(Constants.SuperAdminPolicy);
                });

                options.AddPolicy(Constants.ManagerPolicy, policy =>
                {
                    policy.RequireRole(Constants.SuperAdminPolicy, Constants.ManagerPolicy);
                });
                options.AddPolicy(Constants.AdminPolicy, policy =>
                {
                    policy.RequireRole(Constants.SuperAdminPolicy, Constants.AdminPolicy);
                });
                options.AddPolicy(Constants.AdminOne, policy =>
                {
                    policy.RequireRole(Constants.SuperAdminPolicy, Constants.AdminOne);
                });
                options.AddPolicy(Constants.SliderPolicy, policy =>
                {
                    policy.RequireRole(Constants.SuperAdminPolicy, Constants.SliderPolicy);
                });
                options.AddPolicy(Constants.AdminTwo, policy =>
                {
                    policy.RequireRole(Constants.SuperAdminPolicy, Constants.AdminTwo);
                });
                options.AddPolicy(Constants.AdminThree, policy =>
                {
                    policy.RequireRole(Constants.SuperAdminPolicy, Constants.AdminThree);
                });
                options.AddPolicy(Constants.CompanyPolicy, policy =>
                {
                    policy.RequireRole(Constants.SuperAdminPolicy, Constants.CompanyPolicy);
                });
                options.AddPolicy(Constants.BucketPolicy, policy =>
                {
                    policy.RequireRole(Constants.SuperAdminPolicy, Constants.BucketPolicy);
                });

                options.AddPolicy(Constants.ExchangeRatePolicy, policy =>
                {
                    policy.RequireRole(Constants.SuperAdminPolicy, Constants.ExchangeRatePolicy);
                });
                options.AddPolicy(Constants.AdvertPolicy, policy =>
                {
                    policy.RequireRole(Constants.SuperAdminPolicy, Constants.AdvertPolicy);
                });

                options.AddPolicy(Constants.UsersManagerPolicy, policy =>
                {
                    policy.RequireRole(Constants.SuperAdminPolicy, Constants.UsersManagerPolicy);
                });


                options.AddPolicy(Constants.ClientPolicy, policy =>
                {
                    policy.RequireRole(Constants.SuperAdminPolicy, Constants.ClientPolicy);
                });


                options.AddPolicy(Constants.UserPolicy, policy =>
                {
                    policy.RequireRole(Constants.SuperAdminPolicy, Constants.UserPolicy);
                });
                options.AddPolicy(Constants.PointPolicy, policy =>
                {
                    policy.RequireRole(Constants.SuperAdminPolicy, Constants.PointPolicy);
                });

                options.AddPolicy(Constants.UserPolicy, policy =>
                {
                    policy.RequireRole(Constants.SuperAdminPolicy, Constants.UserPolicy);
                });
                options.AddPolicy(Constants.Dashboard, policy =>
                {
                    policy.RequireRole(Constants.SuperAdminPolicy, Constants.Dashboard, Constants.CompanyPolicy);
                });
                options.AddPolicy(Constants.Transaction, policy =>
                {
                    policy.RequireRole(Constants.SuperAdminPolicy, Constants.Transaction);
                });
                options.AddPolicy(Constants.Permission, policy =>
                {
                    policy.RequireRole(Constants.SuperAdminPolicy, Constants.Permission);
                });
                //options.AddPolicy(Constants.Dashboard, policy =>
                //{
                //    policy.RequireRole(Constants.SuperAdminPolicy, Constants.UserPolicy, Constants.ManagerPolicy,
                //        Constants.AdminPolicy, Constants.CompanyPolicy, Constants.BucketPolicy, Constants.ExchangeRatePolicy,
                //        Constants.AdvertPolicy, Constants.UsersManagerPolicy, Constants.ClientPolicy, Constants.UserPolicy
                //                                );
                //});
                //     [Authorize(Policy = Constants.CompanyPolicy)]

                options.AddPolicy(Constants.ValidatorPolicy, policy =>
                {
                    policy.RequireRole(Constants.ValidatorPolicy);
                });
                // Add more policies as needed
            });

            builder.Services.AddSingleton<LoggerLibrary>(provider =>
            {
                var hostingEnvironment = provider.GetRequiredService<IWebHostEnvironment>();
                var wwwRootPath = hostingEnvironment.WebRootPath;
                return new LoggerLibrary(wwwRootPath);
            });
            builder.Services.AddHttpClient();
             
            // Register PostmarkClient and inject the captured authentication header name
            builder.Services.AddTransient<PostmarkClient>(_ => new PostmarkClient(builder.Configuration.GetSection("PostmarkSettings")["ServerToken"]));
            builder.Services.AddTransient<SymmetricSecurityKeyService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowKoboView",
                    builder =>
                    {
                        builder
                            .WithOrigins("https://koboview.com")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Auth/Account/Login";
                options.LogoutPath = $"/Auth/Account/Logout";
                options.AccessDeniedPath = $"/Auth/Account/AccessDenied";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // Use the CORS policy in your application
            app.UseUserStatusMiddleware();
            app.UseCors("AllowKoboView");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}