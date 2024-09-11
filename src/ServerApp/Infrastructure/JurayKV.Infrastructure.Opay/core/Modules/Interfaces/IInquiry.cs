using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
namespace JurayKV.Infrastructure.Opay.core.Interfaces
{
    public interface IInquiry
    {
        Task<JObject> balanceForAllAccount();
        Task<JObject> validateMerchant(SortedDictionary<String, Object> param);
        Task<JObject> validateUser(SortedDictionary<String, Object> param);
        Task<JObject> verifyAccountAndReturnAllocatedAccountName(SortedDictionary<String, Object> param);
    }
}