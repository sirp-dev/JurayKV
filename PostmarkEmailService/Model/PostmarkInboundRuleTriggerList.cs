using System.Collections.Generic;

namespace PostmarkEmailService.Model
{
    public class PostmarkInboundRuleTriggerList
    {
        public int TotalCount { get; set; }

        public IEnumerable<PostmarkInboundRuleTriggerInfo> InboundRules { get; set; }
    }
}
