using JurayKV.Infrastructure.Services.Configs;
using Microsoft.Extensions.DependencyInjection;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Infrastructure.Services;

public static class ServiceCollectionExtensions
{
    public static void AddSendGrid(this IServiceCollection services, string apiKey)
    {
        services.ThrowIfNull(nameof(services));
        apiKey.ThrowIfNull(nameof(apiKey));

        services.AddSingleton(fact =>
        {
            SendGridConfig sendGridConfig = new(apiKey);
            return sendGridConfig;
        });
    }
}
