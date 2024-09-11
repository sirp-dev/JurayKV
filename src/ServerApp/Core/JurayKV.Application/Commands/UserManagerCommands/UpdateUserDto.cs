using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.Commands.UserManagerCommands
{
    public class UpdateUserDto
    {
        public string SurName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateUpgraded { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public Tier Tier { get; set; }
        public DateTime? LastLoggedInAtUtc { get; set; }
        public DateTime CreationUTC { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool DisableEmailNotification { get; set; }

        public string? ResponseOnTieRequest { get; set; }
        public TieRequestStatus Tie2Request { get; set; }

        public bool EmailComfirmed { get; set; }
        public bool TwoFactorEnable { get; set; }
        public string? NinNumber { get; set; }

    }
}
