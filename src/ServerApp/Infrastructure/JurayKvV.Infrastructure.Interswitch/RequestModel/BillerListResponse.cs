using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.RequestModel
{
   
    public class BillerListResponse
    {
        public BillerListResponseBillerlist BillerList { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseCodeGrouping { get; set; }
    }

    public class BillerListResponseBillerlist
    {
        public int Count { get; set; }
        public BillerListResponseCategory[] Category { get; set; }
    }

    public class BillerListResponseCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public BillerListResponseBiller[] Billers { get; set; }
    }

    public class BillerListResponseBiller
    {
        public string Type { get; set; }
        public int Id { get; set; }
        public int PayDirectProductId { get; set; }
        public int PayDirectInstitutionId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Narration { get; set; }
        public string CustomerField1 { get; set; }
        public string LogoUrl { get; set; }
        public string Surcharge { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public string QuickTellerSiteUrlName { get; set; }
        public BillerListResponsePageflowinfo PageFlowInfo { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int AmountType { get; set; }
        public string CustomerField2 { get; set; }
        public string SupportEmail { get; set; }
        public string RiskCategoryId { get; set; }
        public string Url { get; set; }
        public string CustomSectionUrl { get; set; }
        public string MediumImageId { get; set; }
        public string NetworkId { get; set; }
        public string ProductCode { get; set; }
        public string SmallImageId { get; set; }
        public string LargeImageId { get; set; }
        public string PaymentOptionsPageHeader { get; set; }
        public string CustomMessageUrl { get; set; }
        public string CustomMessageId { get; set; }
        public string PaymentOptionsTitle { get; set; }
    }

    public class BillerListResponsePageflowinfo
    {
        public BillerListResponseElement[] Elements { get; set; }
        public string FinishButtonName { get; set; }
        public string StartPage { get; set; }
        public bool UsesPaymentItems { get; set; }
        public bool PerformInquiry { get; set; }
        public bool AllowRetry { get; set; }
    }

    public class BillerListResponseElement
    {
        public string Title { get; set; }
        public bool IsRequired { get; set; }
        public int SortOrder { get; set; }
        public string ElementType { get; set; }
        public string DataType { get; set; }
        public int Length { get; set; }
        public BillerListResponseSelectitem[] SelectItems { get; set; }
        public string ElementName { get; set; }
        public string SelectItemType { get; set; }
    }

    public class BillerListResponseSelectitem
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

}
