using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Services
{
    // Create a service to generate SymmetricSecurityKey
    public class SymmetricSecurityKeyService
    {
        private readonly IConfiguration _configuration;

        public SymmetricSecurityKeyService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            var secretKey = _configuration["JwtSettings:SecretKey"]; // Example: Read from configuration
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        }
    }
}
