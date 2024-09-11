using System.Collections.Generic;

namespace PostmarkEmailService.Model
{
    public class PostmarkSenderSignatureList
    {
        public int TotalCount { get; set; }
        public IEnumerable<PostmarkBaseSenderSignature> SenderSignatures { get; set; }
    }
}
