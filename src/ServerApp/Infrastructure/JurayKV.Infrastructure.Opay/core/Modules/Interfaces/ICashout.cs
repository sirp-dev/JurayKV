using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace JurayKV.Infrastructure.Opay.core.Interfaces
{
    public interface ICashout
    {
        Task<JObject> initializeTransaction(SortedDictionary<String, Object> param);
        Task<JObject> transactionStatus(SortedDictionary<String, Object> param);
        Task<JObject> closeTransaction(SortedDictionary<String, Object> param);
    }
}