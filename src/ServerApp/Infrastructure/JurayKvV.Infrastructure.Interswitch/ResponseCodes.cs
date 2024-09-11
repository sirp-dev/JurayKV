using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch
{

    public class ResponseCodeHandler
    {
        private Dictionary<string, string> responseCodeMeanings;

        public ResponseCodeHandler()
        {
            responseCodeMeanings = new Dictionary<string, string>
        {
            { "10", "Approved by Financial Institution, Partial" },
            { "11", "Approved by Financial Institution, VIP" },
            { "00", "Approved by Financial Institution" },
            { "09", "Transaction In Progress" },
              { "X00", "Account error, please contact your bank" },
            { "X03", "The amount requested is above the limit permitted by your bank, please contact your bank" },
            { "X04", "The amount requested is too low" },
            { "X05", "The amount requested is above the limit permitted by your bank, please contact your bank" },
            { "14", "The card number inputted is invalid, please re-try with a valid card number" },
            { "38", "Incorrect Security details provided. Pin tries exceeded." },
            { "55", "Incorrect Security details provided." },
            { "56", "Incorrect card details, please verify that the expiry date inputted is correct." },
            { "57", "Your bank has prevented your card from carrying out this transaction, please contact your bank" },
            { "61", "Your bank has prevented your card from carrying out this transaction, please contact your bank" },
            { "75", "Incorrect security details provided. Pin tries exceeded." },
            { "01", "Refer to Financial Institution." },
            { "02", "Refer to Financial Institution, Special Condition" },
            { "03", "Invalid Merchant" },
            { "04", "Pick-up Card" },
            { "05", "Do not honor" },
            { "06", "Error" },
            { "07", "Pick-up Card, Special Condition" },
            { "08", "Honor with identification" },
            { "09", "Request in Progress" },
            { "12", "Invalid Transaction" },
            { "13", "Invalid Amount" },
            { "15", "No Such Financial Institution" },
            { "16", "Approved by Financial Institution, Update Track 3" },
            { "17", "Customer Cancellation" },
            { "18", "Customer Dispute" },
            { "19", "Re-enter Transaction" },
            { "20", "Invalid Response from Financial Institution" },
            { "21", "No Action Taken by Financial Institution" },
            { "22", "Suspected Malfunction" },
            { "23", "Unacceptable Transaction Fee" },
            { "24", "File Update not Supported" },
            { "26", "Duplicate Record" },
            { "27", "File Update File Edit Error" },
            { "28", "File Update File Locked" },
            { "29", "File Update Failed" },
            { "30", "Format Error" },
            { "31", "Bank Not Supported" },
            { "32", "Completed Partially by Financial Institution" },
            { "33", "Institution Expired Card, Pick-Up" },
            { "34", "Suspected Fraud, Pick-Up" },
            { "35", "Contact Acquirer, Pick-Up" },
            { "36", "Restricted Card, Pick-Up" },
            { "37", "Call Acquirer Security, Pick-Up" },
            { "38", "PIN Tries Exceeded, Pick-Up" },
            { "39", "No Credit Account" },
            { "40", "Function not supported" },
            { "41", "Lost Card, Pick-Up" },
            { "42", "No Universal Account" },
            { "44", "No Investment Account" },
            { "51", "Insufficient Funds" },
            { "52", "No Check Account" },
            { "53", "No Savings Account" },
            { "54", "Expired Card" },
            { "55", "Incorrect PIN" },
            { "56", "No Card Record" },
            { "59", "Suspected Fraud" },
            { "60", "Contact Acquirer" },
            { "62", "Restricted Card" },
            { "63", "Security Violation" },
            { "64", "Original Amount Incorrect" },
            { "65", "Exceeds withdrawal frequency" },
            { "66", "Call Acquirer Security" },
            { "67", "Hard Capture" },
            { "68", "Response Received Too Late" },
            { "75", "PIN tries exceeded" },
            { "77", "Intervene, Bank Approval Required" },
            { "78", "Intervene, Bank Approval Required for Partial Amount" },
            { "90", "Cut-off in Progress" },
            { "91", "Issuer or Switch Inoperative" },
            { "92", "Routing Error" },
            { "93", "Violation of law" },
            { "94", "Duplicate Transaction" },
            { "95", "Reconcile Error" },
            { "96", "System Malfunction" },
            { "98", "Exceeds Cash Limit" },
            { "99", "Unexpected error" },
            { "A0", "Transaction not permitted to cardholder, via channels" },
            { "A4", "Transaction Error" },
            { "Z1", "Bank account error" },
            { "Z2", "Bank collections account error" },
            { "Z3", "Interface Integration Error" },
            { "Z4", "Duplicate Reference Error" },
            { "Z5", "Incomplete Transaction" },
            { "Z6", "Transaction Split Pre-processing Error" },
            { "Z7", "Invalid Card Number, via channels" },
            { "Z8", "Transaction not permitted to cardholder, via channels" },
            { "Z9", "Transaction not found" },
            { "Z61", "Payment Requires Token" },
            { "Z62", "Request to Generate Token is Successful" },
            { "Z63", "Token Not Generated. Customer Not Registered on Token Platform" },
            { "Z64", "Error Occurred. Could Not Generate Token" },
            { "Z65", "Payment Requires Token Authorization" },
            { "Z66", "Token Authorization Successful" },
            { "Z67", "Token Authorization Not Successful. Incorrect Token Supplied" },
            { "Z68", "Error Occurred. Could Not Authenticate Token" },
            { "Z69", "Customer Cancellation Secure3D" },
            { "Z70", "Cardinal Authentication Required" },
            { "Z71", "Cardinal Lookup Successful" },
            { "Z72", "Cardinal Lookup Failed" },
            { "Z73", "Cardinal Authenticate Successful" },
            { "Z74", "Cardinal Authenticate Failed" },
            { "Z80", "Error calling Cybersource Service" },
            { "Z81", "Bin has not been configured" },
            { "Z82", "Merchant not configured for bin" },
            { "Z0", "Pending" } // Transaction Status Unconfirmed
        };
        }

        public string GetResponseMeaning(string responseCode)
        {
            if (responseCodeMeanings.ContainsKey(responseCode))
            {
                return responseCodeMeanings[responseCode];
            }
            else
            {
                return "Unknown response code";
            }
        }
    }
}
