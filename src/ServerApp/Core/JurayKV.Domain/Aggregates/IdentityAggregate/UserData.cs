using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.IdentityAggregate
{
    public class UserData
    {
        public Guid Id { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Xvalue { get; set; }
        public string XtxtGuid { get; set; }
        public DateTime XvalueDate { get; set; }
    }
}
