using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application.Queries.BucketQueries
{
    public class BucketDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public bool Disable { get; set; }

        public bool AdminActive { get; set; }

        public bool UserActive { get; set; }

        public DateTime Date { get; set; }
    }
}
