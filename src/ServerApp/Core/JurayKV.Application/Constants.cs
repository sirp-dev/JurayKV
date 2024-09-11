using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Application
{
    public static class Constants
    {
        public const string SuperAdminPolicy = "SuperAdmin";
        public const string AdminPolicy = "Admin";
        public const string ManagerPolicy = "Manager";
        // Add more policies or roles as needed

        public const string CompanyPolicy = "Company";
        public const string BucketPolicy = "Bucket";
        public const string ExchangeRatePolicy = "ExchangeRate";
        public const string AdvertPolicy = "Advert";
        public const string UsersManagerPolicy = "UsersManager";
        public const string ClientPolicy = "Client";
        public const string UserPolicy = "User";
        public const string Dashboard = "User";
        public const string PointPolicy = "Point";
        public const string AdminOne = "AdminOne";
        public const string AdminTwo = "AdminTwo";
        public const string AdminThree = "AdminThree";
        public const string SliderPolicy = "Slider";
        public const string ValidatorPolicy = "Validator";
        public const string Transaction = "Transaction";
        public const string Permission = "Permission";

    }
    public static class EmailMask { 
    public static string MaskEmail(string email)
    {
        int atIndex = email.IndexOf('@');
        if (atIndex < 0)
        {
            // Invalid email address, return it as is
            return email;
        }

        string prefix = email.Substring(0, atIndex);
        string domain = email.Substring(atIndex);

        // Replace characters in the middle with "xxxx"
        int lengthToMask = Math.Max(prefix.Length - 2, 0); // Leave at least 1 character before the '@'
        string maskedPrefix = new string('x', lengthToMask) + email[atIndex - 1];

        return maskedPrefix + domain;
    }
    }

}
