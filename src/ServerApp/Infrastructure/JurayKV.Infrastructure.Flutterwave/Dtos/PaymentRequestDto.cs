using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Flutterwave.Dtos
{
    public class PaymentRequestDto
    {
        public string TxRef { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string RedirectUrl { get; set; }
        public string PaymentOptions { get; set; }
        public Guid ConsumerId { get; set; }
        public string ConsumerMac { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public string From { get; set; }
    }
}
