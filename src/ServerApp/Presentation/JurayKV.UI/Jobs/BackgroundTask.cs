using JurayKV.Infrastructure.Services;

namespace JurayKV.UI.Jobs
{
    public class BackgroundTask : IHostedService, IDisposable
    {
        private readonly ILogger _logger;
        private Timer _timer;
        private readonly IServiceScopeFactory _scopeFactory;
        public BackgroundTask(ILogger<BackgroundTask> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            // Get the current time
            //DateTime now = DateTime.UtcNow.AddHours(1);

            //// Calculate the time until 6 am
            //TimeSpan delayUntil6AM = new DateTime(now.Year, now.Month, now.Day, 6, 0, 0) - now;

            //// If it's already past 6 am, schedule the first run for the next day
            //if (delayUntil6AM.TotalMilliseconds < 0)
            //{
            //    delayUntil6AM = delayUntil6AM.Add(new TimeSpan(24, 0, 0));
            //}

            //// Start the timer with the calculated delay until 6 am
            //_timer = new Timer(DoWork, null, delayUntil6AM, TimeSpan.FromMinutes(2));

            //// Schedule the timer to stop at 6:30 am
            //DateTime stopTime = new DateTime(now.Year, now.Month, now.Day, 6, 30, 0);
            //TimeSpan timeUntilStop = stopTime - now;
            //_timer.Change(timeUntilStop, Timeout.InfiniteTimeSpan);

            // Get the current time
            DateTime now = DateTime.UtcNow.AddHours(1);

            // Calculate the time until 5:40 am
            TimeSpan delayUntil540AM = new DateTime(now.Year, now.Month, now.Day, 9, 20, 0) - now;

            // If it's already past 5:40 am, schedule the first run for the next day
            if (delayUntil540AM.TotalMilliseconds < 0)
            {
                delayUntil540AM = delayUntil540AM.Add(new TimeSpan(24, 0, 0));
            }

            // Start the timer with the calculated delay until 5:40 am
            Timer timer = new Timer(DoWork, null, delayUntil540AM, TimeSpan.FromMinutes(2));

            // Schedule the timer to stop at 5:50 am
            DateTime stopTime = new DateTime(now.Year, now.Month, now.Day, 9, 50, 0);
            TimeSpan timeUntilStop = stopTime - now;
            //timer.Change(timeUntilStop, Timeout.InfiniteTimeSpan);
            // Ensure dueTime is non-negative and within the valid range
            long dueTimeInMilliseconds = (long)timeUntilStop.TotalMilliseconds;
            if (dueTimeInMilliseconds < 0 || dueTimeInMilliseconds > int.MaxValue)
            {
                // Handle the error, set a default due time, or throw an exception
                // For example, you can set a default due time of 0 or use a different strategy based on your requirements.
                dueTimeInMilliseconds = 0;
            }

            // Configure the timer to stop at the specified time
            timer.Change(dueTimeInMilliseconds, Timeout.Infinite);
            #region
            //// Get the current time
            //DateTime now = DateTime.UtcNow.AddHours(1);

            //// Calculate the time until 6 AM tomorrow
            //DateTime nextSixAM = new DateTime(now.Year, now.Month, now.Day, 6, 0, 0);
            //if (now > nextSixAM)
            //{
            //    // If it's already past 6 AM, schedule it for the next day
            //    nextSixAM = nextSixAM.AddDays(1);
            //}

            //// Calculate the time until the next 6 AM
            //TimeSpan initialDelay = nextSixAM - now;

            //// Create a timer that will execute the specified method at 6 AM every day
            ////////////////Timer timer = new Timer(DoWork, null, TimeSpan.zer, TimeSpan.FromHours(24));
            //Timer timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(2));
            //// Calculate the time until 5 AM tomorrow
            //DateTime next5AM = new DateTime(now.Year, now.Month, now.Day, 6, 0, 0);
            //if (now > next5AM)
            //{
            //    // If it's already past 5 AM, schedule it for the next day
            //    next5AM = next5AM.AddDays(1);
            //}
            //// Calculate the time until the next 5 AM
            //TimeSpan initialDelay2 = next5AM - now;

            //_timer = new Timer(DoWorkMorningMail, null, initialDelay2, TimeSpan.FromHours(24));

            //// Calculate the time until the next 6:00 PM
            //DateTime nextSixPM = new DateTime(now.Year, now.Month, now.Day, 18, 0, 0);
            //if (now > nextSixPM)
            //{
            //    // If it's already past 6:00 PM today, schedule for tomorrow
            //    nextSixPM = nextSixPM.AddDays(1);
            //}

            //// Calculate the initial delay for the evening task
            //TimeSpan initialEveningDelay = nextSixPM - now;

            //// Create a Timer that executes DoWorkEveningMail every day from 6:00 PM to 8:00 PM
            //_timer = new Timer(DoWorkEveningMail, null, initialEveningDelay, TimeSpan.FromHours(24));
            #endregion
            return Task.CompletedTask;
        }
        private async void DoWork(object state)
        {
            _logger.LogInformation("Timed Background Service is working.");

            using (var scope = _scopeFactory.CreateScope())
            {
                var backgroundActivity = scope.ServiceProvider.GetRequiredService<BackgroundActivity>();
                //await backgroundActivity.UpdateAllUserAdsAfterSix();
            }
        }
        private async void DoWorkEveningMail(object state)
        {
            _logger.LogInformation("Timed Background Service is working.");

            using (var scope = _scopeFactory.CreateScope())
            {
                var backgroundActivity = scope.ServiceProvider.GetRequiredService<BackgroundActivity>();
                await backgroundActivity.SendEmailToEveningActiveAdsAsync();
            }
        }

        private async void DoWorkMorningMail(object state)
        {
            _logger.LogInformation("Timed Background Service is working.");

            using (var scope = _scopeFactory.CreateScope())
            {
                var backgroundActivity = scope.ServiceProvider.GetRequiredService<BackgroundActivity>();
                await backgroundActivity.SendEmailToMorningReminderAsync();
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
