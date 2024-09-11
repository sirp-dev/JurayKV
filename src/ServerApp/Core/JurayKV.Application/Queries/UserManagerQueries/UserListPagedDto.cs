using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Queries.UserManagerQueries
{
    public class UserListPagedDto
    {
        public List<UserManagerListDto> UserManagerListDto { get; set; }
        public int TotalCount {  get; set; }
        public string? DistintPhone { get; set; }
        public int DistintPhoneCount { get; set; }
        public int DistintPhoneCountActive { get; set; }
        public string? DistintEmail { get; set; }
        public int DistintEmailCount { get; set; }
        public int DistintEmailCountActive { get; set; } 

    }
}
