using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using JurayKV.Infrastructure.Opay.core.Infrastructure;
using JurayKV.Infrastructure.Opay.core.Interfaces;
using JurayKV.Infrastructure.Opay.core.modules.Infrastructure;

namespace JurayKV.Infrastructure.Opay.core.Modules
{
    public class Cashout : ICashout
    {
    private IConnectionClient _connectionClient;

        private readonly String BASEURL = "http://sandbox.cashierapi.operapay.com/api/v3";

        private readonly String MERCHANTID = "256620112018031";
        private readonly String PUBLICKEY = "OPAYPUB16058777635980.9961229244591103";
        private readonly String PRIVATEKEY = "OPAYPRV16058777635980.3804652128291669";

        public Cashout(IConnectionClient connectionClient)
    {
        this._connectionClient = connectionClient;
    }

    //public Task<JObject> initializeTransaction(SortedDictionary<String, Object> param) {
    //        return _connectionClient.makePostRequestAsync(param, Endpoint.OPAY_CHECKOUT_INITIALIZE_TRANSACTION);
    //}

    //public Task<JObject> transactionStatus(SortedDictionary<String, Object> param) {
    //        return _connectionClient.makePostRequestAsync(param, Endpoint.OPAY_CHECKOUT_TRANSACTION_STATUS);
    //}

    //public Task<JObject> closeTransaction(SortedDictionary<String, Object> param) {
    //    return _connectionClient.makePostRequestAsync(param,Endpoint.OPAY_CHECKOUT_CLOSE_TRANSACTION);
    //}
    }
}