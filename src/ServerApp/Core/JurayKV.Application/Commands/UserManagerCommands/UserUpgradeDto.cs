using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Commands.UserManagerCommands
{
    public class UserUpgradeDto
    {
        public string? About { get; set; }
        public string? AlternativePhone { get; set; }
        public string? Address { get; set; }
        public string? State { get; set; }
        public string? LGA { get; set; }
        public string? Occupation { get; set; }
        public string? FbHandle { get; set; }
        public string? InstagramHandle { get; set; }
        public string? TwitterHandle { get; set; }
        public string? TiktokHandle { get; set; }
    }
}
