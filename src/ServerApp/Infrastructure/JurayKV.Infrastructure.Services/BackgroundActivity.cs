using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Infrastructures;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Persistence.RelationalDB;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Services
{

    public sealed class BackgroundActivity : IBackgroundActivity
    {
        private readonly IConfiguration _configManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly JurayDbContext _dbContext;
        private readonly IIdentityKvAdCacheHandler _departmentCacheHandler;
        private readonly IEmailSender _email;
        public BackgroundActivity(IExceptionLogger exceptionLogger, UserManager<ApplicationUser> userManager, IConfiguration configManager, JurayDbContext dbContext, IIdentityKvAdCacheHandler departmentCacheHandler, IEmailSender email)
        {
            _exceptionLogger = exceptionLogger;
            _userManager = userManager;
            _configManager = configManager;
            _dbContext = dbContext;
            _departmentCacheHandler = departmentCacheHandler;
            _email = email;
        }

        public async Task SendEmailToEveningActiveAdsAsync()
        {
            DateTime currentDate = DateTime.UtcNow.AddHours(1);
            DateTime nextDay6AM = currentDate.Date.AddDays(1).AddHours(6);

            //var data = _dbContext.IdentityKvAds.Include(x => x.KvAd)
            //.Where(x => x.CreatedAtUtc < nextDay6AM)
            //.Where(x => x.Active == true);

            var user = await _userManager.Users.Where(x=>x.DisableEmailNotification == true).ToListAsync();

            string msg = "Kindly Upload the proof of your status before 6am tomorrow, or upload it now <br> if you are yet to upload status. Kindly do so before the todays advert end by 6am.";
            foreach ( var item in user )
            {
                await _email.SendAsync(msg, item.Id.ToString(), "Upload Status Proof before 6am");
            }
        }

        public async Task SendEmailToMorningReminderAsync()
        {
             
            
            var user = await _userManager.Users.Where(x => x.DisableEmailNotification == true).ToListAsync();

            string msg = "Kindly Upload the proof of your status before 6am tomorrow, or upload it now. <br> Be ready to pick the new advert for today";
            foreach (var item in user)
            {
                await _email.SendAsync(msg, item.Id.ToString(), "Upload Status Proof before 6am");
            }
        }

        public async Task UpdateAllUserAdsAfterSix()
        {
            DateTime cutoffTime = DateTime.UtcNow.AddHours(1).Date.AddHours(6);
            var data = await _dbContext.IdentityKvAds.AsNoTracking()
                .Where(msg => msg.CreatedAtUtc < cutoffTime && msg.Active == true) 
                .Where(x => x.Active == true).Take(50).ToListAsync();
            foreach (var item in data)
            {
                item.Active = false;
                item.AdsStatus = Domain.Primitives.Enum.AdsStatus.Void;
                _dbContext.Attach(item).State = EntityState.Modified;
            }
            await _dbContext.SaveChangesAsync();

            await _departmentCacheHandler.RemoveListAsync();
            await _departmentCacheHandler.RemoveListActiveTodayAsync();

            foreach (var item in data)
            {
                await _departmentCacheHandler.RemoveGetByUserIdAsync(item.UserId);
                await _departmentCacheHandler.RemoveGetActiveByUserIdAsync(item.UserId);
                await _departmentCacheHandler.RemoveDetailsByIdAsync(item.Id);
                await _departmentCacheHandler.RemoveGetAsync(item.Id);
            }

        }
    }
}
