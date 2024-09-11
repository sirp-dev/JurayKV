using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Commands.IdentityCommands.UserCommands
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public bool Verified { get; set; }
    }
}
