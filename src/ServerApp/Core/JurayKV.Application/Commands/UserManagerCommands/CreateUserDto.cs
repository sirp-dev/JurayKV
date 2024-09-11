using System.ComponentModel.DataAnnotations;

namespace JurayKV.Application.Commands.UserManagerCommands
{
    public class CreateUserDto
    {
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
         public bool Comfirm { get; set; }
        public string Role { get; set; }
        public string RefPhone { get; set; }

         public string State { get; set; }

         public string LGA { get; set; }


         public string Address { get; set; }
    }
}
