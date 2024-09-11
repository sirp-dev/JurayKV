using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Domain.Primitives
{
    public class Enum
    {
        public enum PaymentGateway
        {
            [Description("NotDefind")]
            NotDefind = 0,
            [Description("Flutterwave")]
            Flutterwave = 1,

            [Description("Interswitch")]
            Interswitch = 2,
            Bank = 5,
        }
        public enum Tier
        {
            [Description("Tier1")]
            Tier1 = 0,
            [Description("Tier2")]
            Tier2 = 1, 
        }
        
        public enum BillGateway
        {
            [Description("NotDefind")]
            NotDefind = 0,
            [Description("Flutterwave")]
            Flutterwave = 1,

            [Description("Interswitch")]
            Interswitch = 2,

            [Description("VTU")]
            VTU = 4,
        }
        public enum NotificationStatus
        {
            [Description("NotDefind")]
            NotDefind = 0,
            [Description("Sent")]
            Sent = 1,

            [Description("NotSent")]
            NotSent = 2,


        }
        public enum NotificationType
        {
            [Description("NotDefind")]
            NotDefind = 0,
            [Description("SMS")]
            SMS = 1,

            [Description("Email")]
            Email = 2,

            [Description("Voice")]
            Voice = 3,
            [Description("Whatsapp")]
            Whatsapp = 4
                
        }

        public enum CartStatus
        {
            [Description("NotDefind")]
            NotDefind = 0,
            [Description("Active")]
            Active = 1,

            [Description("CheckOut")]
            CheckOut = 2


        }

        public enum UtilityParam
        {
            [Description("NotDefind")]
            NotDefind = 0,
            [Description("Data")]
            Data = 1,

            [Description("Airtime")]
            Airtime = 2,

            [Description("Electricity")]
            Electricity = 3,

            [Description("Cable")]
            Cable = 4


        }
      
        public enum ContentType
        {
            [Description("NotDefind")]
            NotDefind = 0,
            [Description("OrderFromShopMail")]
            OrderFromShopMail = 1,

            [Description("OrderFromShopSMS")]
            OrderFromShopSMS = 2,
            [Description("OrderFromCustomerMail")]
            OrderFromCustomerMail = 3,
            [Description("OrderFromCustomerSMS")]
            OrderFromCustomerSMS = 4,

            [Description("OrderAdminMail")]
            OrderAdminMail = 5,
            [Description("OrderAdminSMS")]
            OrderAdminSMS = 6,

            [Description("OnlineDepositSMS")]
            OnlineDepositSMS = 7,
            [Description("OnlineDepositEmail")]
            OnlineDepositEmail = 8,

            [Description("AhiaPayTransferAfterProcessingOrderSms")]
            AhiaPayTransferAfterProcessingOrderSms = 9,
            [Description("AhiaPayTransferAfterProcessingOrderEmail")]
            AhiaPayTransferAfterProcessingOrderEmail = 10,

            [Description("TransferToBankSms")]
            TransferToBankSms = 11,
            [Description("TransferToBankEmail")]
            TransferToBankEmail = 12,

            [Description("SoaRequestSms")]
            SoaRequestSms = 13,
            [Description("SoaRequestEmail")]
            SoaRequestEmail = 14


        }
     

        public enum MessageStatus
        {
            [Description("NotDefind")]
            NotDefind = 0,
            [Description("SMS")]
            SMS = 1,

            [Description("Mail")]
            Mail = 2


        }

        public enum ChangeDataStatus
        {
            [Description("NotDefind")]
            NotDefind = 0,
            [Description("Successful")]
            Successful = 1,

            [Description("Canceled")]
            Canceled = 2,
            [Description("Pending")]
            Pending = 3,


        }

        public enum TieRequestStatus
        {
            [Description("None")]
            None = 0,
            [Description("Requested")]
            Requested = 1,

            [Description("Approved")]
            Approved = 2,
            [Description("Cancelled")]
            Cancelled = 3,

        }

        public enum AccountStatus
        {
            [Description("NotDefind")]
            NotDefind = 0,
            [Description("Active")]
            Active = 1, 

            [Description("Disabled")]
            Disabled = 2,

            [Description("Suspended")]
            Suspended = 3,

            [Description("NotActive")]
            NotActive = 4,

            [Description("New")]
            New = 5,
        }

        public enum TransactionTypeEnum
        {
            Debit = 1,
            Credit = 2,
             
        }
      
       
        public enum TransferEnum
        {
            Processed = 5,
            Success = 1,
            Pending = 2,
            Fail = 3,
            Reversed = 4
        }

        public enum FaqType
        {
            [Description("Delivery")]
            Delivery = 1,

            [Description("Shops")]
            Shops = 2,

            [Description("Product")]
            Product = 3,


            [Description("SOA")]
            SOA = 8,
            [Description("Customer")]
            Customer = 9,
        }

        public enum PageType
        {
            [Description("Normal")]
            Normal = 10,

            [Description("Security")]
            Security = 20,

            [Description("Others")]
            Others = 30,


        }

        public enum PayoutStatus
        {

            None = 0,
            Reversed = 10,
            Ledger = 20,
            Available = 30,


        }
        public enum DataStatus
        {
            [Description("Active")]
            Active = 1,

            [Description("NotActive")]
            NotActive = 2,
             


        }
        public enum VariationType
        {
            [Description("Airtime/Data")]
            AirtimeData = 1,

            [Description("TV")]
            TV = 2,

            [Description("Electricity")]
            Electricity = 3,


        }
        public enum EntityStatus
        {
            [Description("Successful")]
            Successful = 1,

            [Description("Pending")]
            Pending = 2,

            [Description("Suspended")]
            Suspended = 3,

            
        }
        public enum AdsStatus
        {
            [Description("Credited")]
            Credited = 1,

            [Description("Pending")]
            Pending = 0,

            [Description("Suspended")]
            Suspended = 3,
            [Description("Cancelled")]
            Cancelled = 2,
            [Description("Void")]
            Void = 4,


        }

        public enum MarketType
        {
            None = 0,
            Market = 1,

            Road = 2,

            Street = 3,

            Junction = 4,
            Mall = 5
        }

        public enum QueueStatus
        {
            None = 0,
            Success = 1,

            Cancel = 2,

            Pending = 3,

        }
        public enum OrderStatus
        {
            None = 0,
            Pending = 9,
            Completed = 1,
            Reversed = 3,
            Processing = 4,
            Cancel = 6,
            OutOfStock = 7
        }

        public enum ShopStatus
        {
            None = 0,
            Pending = 1,
            Successful = 2,
            Cancel = 3
        }

        public enum LogisticStatus
        {
            None = 0,
            Pending = 1,
            Successful = 2,
            Cancel = 3
        }
        public enum CustomerStatus
        {
            None = 0,
            Pending = 1,
            Successful = 2,
            Cancel = 3
        }


        public enum SendNotification
        {
            No = 1,

            Yes = 2,

            Sent = 3
        }
        public enum PropertyStatus
        {
            [Description("None")]
            None = 0,
            [Description("Active")]
            Active = 1,
            [Description("Bidding")]
            Bidding = 2,
            [Description("Sold")]
            Sold = 3,
            [Description("Pending")]
            Pending = 4,
            [Description("Delete")]
            Delete = 5,



        }

        public enum HomeLocation
        {
            [Description("None")]
            None = 0,
            [Description("MajorMain")]
            MajorMain = 1,
            [Description("MinnorMain")]
            MinnorMain = 2,
            [Description("List")]
            List = 3,

        }

        public enum PagePosition
        {
            [Description("None")]
            None = 0,
            [Description("Head")]
            Head = 1,
            [Description("Footer")]
            Footer = 2,
            [Description("Last Footer")]
            LastFooter = 3,

        }

        public enum PageStatus
        {
            [Description("None")]
            None = 0,
            [Description("Active")]
            Active = 1,
            [Description("Disabled")]
            Disabled = 2

        }

        public enum VerificationStatus
        {
            [Description("None")]
            None = 0,
            [Description("Verified")]
            Verified = 1,

            [Description("Not Verified")]
            NotVerified = 2,



        }

        public enum DescriptiveStatus
        {
            [Display(Description = "None")]
            None = 0,
            [Display(Description = "For Rent")]
            ForRent = 1,

            [Display(Description = "For Sale")]
            ForSale = 2,

            [Display(Description = "Short Let")]
            ShortLet = 3,

        }

        public enum SmsStatusEnum
        {
            [Description("Sent")]
            Sent = 1,

            [Description("Pending")]
            Pending = 2,

            [Description("Not Sent")]
            NotSent = 3,

        }

        public enum AgentStatusEnum
        {
            [Description("Approved")]
            Approved = 1,

            [Description("Pending")]
            Pending = 2,

        }

        public enum ModeOfId
        {
            [Description("None")]
            None = 0,

            [Description("Driver Licence")]
            DriverLicence = 1,

            [Description("National Id")]
            NationalId = 2,
            [Description("International Passport")]
            InternationalPassport = 3,

        }

        public enum UserVerification
        {
            [Description("Verified")]
            Verified = 1,

            [Description("Unverified")]
            Unverified = 2,

        }

        public enum BannerType
        {
            [Description("Washington")]
            Washington = 100,
            [Description("Beijing")]
            Beijing = 200,
            [Description("Tokyo")]
            Tokyo = 300,
            [Description("Berlin")]
            Berlin = 400,
            [Description("NewDelhi")]
            NewDelhi = 500,
            [Description("London")]
            London = 600,
            [Description("Mobile")]
            Mobile = 900,
            [Description("WebMobile")]
            WebMobile = 940,



        }

        public enum PayoutEnum
        {
            [Description("PayedToBank")]
            PayedToBank = 1,

            [Description("Pending")]
            Pending = 2,
            [Description("PayedToWallet")]
            PayedToWallet = 3,

        }

        public enum SideBarnerEnum
        {
            [Description("none")]
            none = 0,


            [Description("Side Bar Big")]
            SideBarBig = 10,

            [Description("Side Bar Small")]
            SideBarSmall = 20,

            [Description("Winning Location")]
            WinningLocation = 30,

        }
        //    PayU
        //Transfer
        //Flutter wave
        //Crypto currencies
        //All payment systems
        public enum PaymentEnum
        {
            [Description("none")]
            none = 0,

            [Description("PayU")]
            PayU = 10,

            [Description("FlutterWave")]
            FlutterWave = 20,
            [Description("CryptoCurrencies")]
            CryptoCurrencies = 30,

            [Description("All")]
            All = 50,

        }
        public enum DeliveryEnum
        {
            [Description("none")]
            none = 0,

            [Description("Self")]
            Self = 10,

            [Description("XYZLogistics")]
            XYZLogistics = 20,
            [Description("UPS")]
            UPS = 30,
            [Description("URU")]
            URU = 40,

            [Description("All")]
            All = 50,

        }

        public enum VehicleEnum
        {
            [Description("none")]
            none = 0,


            [Description("Active")]
            Active = 10,

            [Description("Disabled")]
            Disabled = 20,

        }
        public enum LogisticEnum
        {
            [Description("none")]
            none = 0,


            [Description("Active")]
            Active = 10,

            [Description("Disabled")]
            Disabled = 20,

        }

        public enum TenantEnum
        {
            [Description("none")]
            none = 0,


            [Description("Disable")]
            Disable = 10,

            [Description("Enable")]
            Enable = 20,

            [Description("Suspend")]
            Suspend = 30,

        }
        public enum SoaRequestStatus
        {
            [Description("none")]
            none = 0,


            [Description("Assigned")]
            Assigned = 10,

            [Description("NoAssigned")]
            NoAssigned = 20,
            [Description("Processed")]
            Processed = 20,
        }


    }
}
