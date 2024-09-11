using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.RequestModel
{
    public class PaymentRequest
    {
        public string merchant_code {  get; set; }
        public string pay_item_id {  get; set; }
        public string txn_ref {  get; set; }
        public int amount {  get; set; }
        public string currency {  get; set; }
        public string cust_name {  get; set; }
        public string cust_email {  get; set; }
        public string cust_id {  get; set; }
        public string pay_item_name {  get; set; }
        public string site_redirect_url {  get; set; }
        
    }
}
