using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Persistence.RelationalDB.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void ApplyDatabaseMigrations(this WebApplication app)
    {
        app.ThrowIfNull(nameof(app));

        using IServiceScope serviceScope = app.Services.CreateScope();
        JurayDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<JurayDbContext>();

        dbContext.Database.Migrate();
    }
}
