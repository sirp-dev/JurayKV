using JurayKV.Domain.Aggregates.IdentityAggregate;
using Microsoft.AspNetCore.Identity;

namespace JurayKV.UI.Services
{
     
    public class UserStatusMiddleware
    {
        private readonly RequestDelegate _next;

        public UserStatusMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var userManager = context.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
            var signInManager = context.RequestServices.GetRequiredService<SignInManager<ApplicationUser>>();

            var userLogin = context.User.Identity.Name;

            if (!string.IsNullOrEmpty(userLogin))
            {
                var user = await userManager.FindByNameAsync(userLogin);

                if (user != null && user.AccountStatus == Domain.Primitives.Enum.AccountStatus.Suspended)
                {
                    await signInManager.SignOutAsync();
                    context.Response.Redirect("/Auth/Account/Locked?id=" + user.Id);
                    return;
                }
            }

            await _next(context);
        }
    }

    public static class UserStatusMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserStatusMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserStatusMiddleware>();
        }
    }
}
