using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Services
{
    public class LoggerLibrary
    {
        private readonly string logFilePath;
        //private readonly JurayUniversal.Persistence.EF.SQL.DashboardDbContext _context;

        public LoggerLibrary(string wwwRootPath, string logFileName = "datalog.txt")
        {
            // Combine the content root path with the log file name
            logFilePath = Path.Combine(wwwRootPath, logFileName);

        }

        public async Task Log(string message)
        {
            //var setting = await _context.SuperSettings.FirstOrDefaultAsync();
            string logMessage = $"{DateTime.UtcNow.AddHours(1)} - log {message}\n";

            // Append the log message to the file
            File.AppendAllText(logFilePath, logMessage);
        }
    }
}
