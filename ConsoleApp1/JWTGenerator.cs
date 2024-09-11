using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Security.Cryptography;
namespace ConsoleApp1
{

   
    public static class JWTGenerator
    {
        public static string Generate()
        {
            byte[] keyBytes = GenerateRandomKey();
            string base64Key = Convert.ToBase64String(keyBytes);

            return base64Key;
        }

        static byte[] GenerateRandomKey()
        {
            using (var hmac = new HMACSHA256())
            {
                return hmac.Key;
            }
        }
    }
}
