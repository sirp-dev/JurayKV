using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using JurayKV.Infrastructure.Opay.core.Infrastructure;
using JurayKV.Infrastructure.Opay.core.Interfaces;
using JurayKV.Infrastructure.Opay.core.modules.Infrastructure;

namespace JurayKV.Infrastructure.Opay.core.Modules
{
    public class Inquiry : IInquiry
    {
     private IConnectionClient _connectionClient;

    public Inquiry(IConnectionClient connectionClient)
    {
        this._connectionClient = connectionClient;
    }

    public Task<JObject> balanceForAllAccount() {
        return this._connectionClient.makePostRequestAsync(Endpoint.OPAY_INQUIRY_BALANCE_FOR_ALL_ACCOUNT);
    }

    public Task<JObject> validateMerchant(SortedDictionary<String, Object> param) {
        return this._connectionClient.makePostRequestAsync(param,Endpoint.OPAY_INQUIRY_VALIDATE_MERCHANT);
    }

    public Task<JObject> validateUser(SortedDictionary<String, Object> param) {
        return this._connectionClient.makePostRequestAsync(param,Endpoint.OPAY_INQUIRY_VALIDATE_USER);
    }

    public Task<JObject> verifyAccountAndReturnAllocatedAccountName(SortedDictionary<String, Object> param) {
        return this._connectionClient.makePostRequestAsync(param,Endpoint.OPAY_INQUIRY_VERIFY_ACCOUNT_AND_RETURN_ALLOCATEDACCOUNTNAME);
    }
    }
}