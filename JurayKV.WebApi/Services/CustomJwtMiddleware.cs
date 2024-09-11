using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JurayKV.WebApi.Services
{
    public class CustomJwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CustomJwtMiddleware> _logger;
        private readonly PathString[] _allowedPaths;
        public CustomJwtMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<CustomJwtMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
            _allowedPaths = new PathString[] { "/api/login", "/api/register" }; // Add more paths if needed
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                await ValidateTokenAsync(token, context);
            }
            else
            {
                _logger.LogError("Authorization token is missing.");
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Authorization token is missing.");
                return;
            }

            await _next(context);
        }

        private async Task ValidateTokenAsync(string token, HttpContext context)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JwtSecurityToken:Key"]);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration["JwtSecurityToken:Issuer"],
                    ValidAudience = _configuration["JwtSecurityToken:Audience"],
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                _logger.LogInformation("Token is valid.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Token validation failed: {ex.Message}");
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync($"Token validation failed: {ex.Message}");
            }
        }
    }

    public static class CustomJwtMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomJwtMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomJwtMiddleware>();
        }
    }
}
