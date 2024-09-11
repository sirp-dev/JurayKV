using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.ResponseModel
{

    public class BillersByCategoryResponse
    {
        public BillersByCategoryResponseBillerlist BillerList { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseCodeGrouping { get; set; }
    }

    public class BillersByCategoryResponseBillerlist
    {
        public int Count { get; set; }
        public BillersByCategoryResponseCategory[] Category { get; set; }
    }

    public class BillersByCategoryResponseCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public BillersByCategoryResponseBiller[] Billers { get; set; }
    }

    public class BillersByCategoryResponseBiller
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
        public BillersByCategoryResponsePageflowinfo PageFlowInfo { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int AmountType { get; set; }
        public string CustomerField2 { get; set; }
        public string SupportEmail { get; set; }
        public string RiskCategoryId { get; set; }
        public string Url { get; set; }
        public string CustomSectionUrl { get; set; }
        public string MediumImageId { get; set; }
    }

    public class BillersByCategoryResponsePageflowinfo
    {
        public BillersByCategoryResponseElement[] Elements { get; set; }
        public string FinishButtonName { get; set; }
        public string StartPage { get; set; }
        public bool UsesPaymentItems { get; set; }
        public bool PerformInquiry { get; set; }
        public bool AllowRetry { get; set; }
    }

    public class BillersByCategoryResponseElement
    {
        public string Title { get; set; }
        public bool IsRequired { get; set; }
        public int SortOrder { get; set; }
        public string ElementType { get; set; }
        public string DataType { get; set; }
        public int Length { get; set; }
        public object[] SelectItems { get; set; }
        public string ElementName { get; set; }
    }

}
