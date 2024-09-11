using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Commands.UserManagerCommands
{
    public class UpdateProfileDto
    {
        public string? AccountNumber { get; set; }
        public string? AccountName { get; set; }
        public string? BankName { get; set; }
        public string? BVN { get; set; }
        public string? Surname { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Address { get; set; }
        public string? State { get; set; }
        public string? LGA { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? StateOfOrigin { get; set; }
        public string? LGA_Of_Origin { get; set; }
        public string? Occupation { get; set; }
        public string? About { get; set; }
    }
}
