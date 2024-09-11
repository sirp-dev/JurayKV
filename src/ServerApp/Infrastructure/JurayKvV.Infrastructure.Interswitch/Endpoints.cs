namespace JurayKvV.Infrastructure.Interswitch
{
    public class Endpoints
    {
        //live url
        //public const string BaseUrl = "https://webpay.interswitchng.com";

        //test url
        //public const string BaseUrl = "https://qa.interswitchng.com";



        /// <summary>
        /// live
        //public const string Pay = "https://newwebpay.interswitchng.com/collections/w/pay'";


        /// </summary>
        /// 


        ///
        //
        //test mode
        public const string Pay = "https://newwebpay.qa.interswitchng.com/collections/w/pay";
        public const string ConfirmingTransactionStatus = "https://qa.interswitchng.com/collections/api/v1/gettransaction.json?merchantcode={merchantcode}&transactionreference={reference}&amount={amount}";
        //

       
    }
}
