using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using JurayKV.Application.Services;
using Microsoft.Extensions.Configuration;
namespace JurayKV.Application.Commands.IdentityCommands.UserCommands
{
    public sealed class LoginCommand : IRequest<LoginResponse> // Change return type to string for token
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse> // Change return type to string for token
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SymmetricSecurityKeyService _symmetricSecurityKeyService;
        public IConfiguration _configuration;
        public LoginCommandHandler(UserManager<ApplicationUser> userManager, SymmetricSecurityKeyService symmetricSecurityKeyService, IConfiguration configuration)
        {
            _userManager = userManager;
            _symmetricSecurityKeyService = symmetricSecurityKeyService;
            _configuration = configuration;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    if (user.EmailConfirmed == false)
                    {
                        return new LoginResponse
                        {
                            Verified = false,
                            UserId = user.Id,
                            Email = user.Email,
                        };
                    }
                    else
                    {
                        string token = GenerateToken(user);

                        return new LoginResponse
                        {
                            Token = token,
                            UserId = user.Id,
                            Email = user.Email,
                            Verified = true
                        };
                    }

                }
            }
            catch
            {
                // Handle exceptions
            }
            return null;
        }

        private string GenerateToken(ApplicationUser user)
        {

            var nowUtc = DateTime.UtcNow.AddHours(1);
            var expirationDuration = TimeSpan.FromMinutes(60); // Adjust as needed
            var expirationUtc = nowUtc.Add(expirationDuration);

            var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["JwtSecurityToken:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(nowUtc).ToString(), ClaimValueTypes.Integer64),
                        new Claim(JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(expirationUtc).ToString(), ClaimValueTypes.Integer64),
                        new Claim(JwtRegisteredClaimNames.Iss, _configuration["JwtSecurityToken:Issuer"]),
                        new Claim(JwtRegisteredClaimNames.Aud, _configuration["JwtSecurityToken:Audience"]),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim("Username", user.Email)
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityToken:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSecurityToken:Issuer"],
                audience: _configuration["JwtSecurityToken:Audience"],
                claims: claims,
                expires: expirationUtc,
                signingCredentials: signIn);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;















            //    var claims = new[]
            //    {
            //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //    new Claim(ClaimTypes.Email, user.Email)
            //    // Add additional claims if needed
            //};

            //    var signinCredentials = new SigningCredentials(_symmetricSecurityKeyService.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256);
            //    var jwtSecurityToken = new JwtSecurityToken(
            //        issuer: "JURAYSMARTSOLUTIONFCTABUJA",
            //        audience: "https://koboview.com",
            //        claims: claims,
            //        expires: DateTime.Now.AddMinutes(60),
            //        signingCredentials: signinCredentials
            //    );
            //    return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
