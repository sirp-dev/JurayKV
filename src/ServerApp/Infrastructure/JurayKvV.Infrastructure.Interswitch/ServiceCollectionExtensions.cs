using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInterswitch(
        this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<Authentication>(_ => new Authentication(configuration));


        }
    }
}
