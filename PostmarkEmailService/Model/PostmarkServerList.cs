using System.Collections.Generic;

namespace PostmarkEmailService.Model
{
    public class PostmarkServerList
    {
        public int TotalCount { get; set; }
        public IEnumerable<PostmarkServer> Servers { get; set; }
    }
}
