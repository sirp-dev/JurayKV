using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Queries.IdentityQueries.PermissionQueries
{
    public class UserPermissionDto
    {
        public string UserId { get; set; }
        public string IdNumber { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public List<RoleList> Roles { get; set; }
    }

    public class RoleList
    {
        public string Role { get; set; }
        public string RoleId { get; set; }
        public bool Selected { get; set; }
    }
}
