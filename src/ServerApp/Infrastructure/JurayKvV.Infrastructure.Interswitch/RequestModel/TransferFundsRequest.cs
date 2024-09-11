using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.RequestModel
{
    public class TransferFundsRequest
    {
        public string TransferCode { get; set; }
        public string Mac { get; set; }
        public Termination Termination { get; set; }
        public Sender Sender { get; set; }
        public string InitiatingEntityCode { get; set; }
        public Initiation Initiation { get; set; }
        public Beneficiary Beneficiary { get; set; }
    }

    public class Termination
    {
        public string Amount { get; set; }
        public AccountReceivable AccountReceivable { get; set; }
        public string EntityCode { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentMethodCode { get; set; }
        public string CountryCode { get; set; }
    }

    public class AccountReceivable
    {
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
    }

    public class Sender
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Lastname { get; set; }
        public string Othernames { get; set; }
    }

    public class Initiation
    {
        public string Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string PaymentMethodCode { get; set; }
        public string Channel { get; set; }
    }

    public class Beneficiary
    {
        public string Lastname { get; set; }
        public string Othernames { get; set; }
    }

}
