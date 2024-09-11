using System.Threading.Tasks;
using JurayKV.Blazor.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TanvirArjel.Blazor.DependencyInjection;

namespace JurayKV.Blazor;

public static class Program
{
    public static async Task Main(string[] args)
    {
        WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddComponents();

        builder.Services.AddDependencyServices();

        await builder.Build().RunAsync();
    }
}
