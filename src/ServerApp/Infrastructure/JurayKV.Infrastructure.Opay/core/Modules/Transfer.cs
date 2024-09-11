using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using JurayKV.Infrastructure.Opay.core.Infrastructure;
using JurayKV.Infrastructure.Opay.core.Interfaces;
using JurayKV.Infrastructure.Opay.core.modules.Infrastructure;

namespace JurayKV.Infrastructure.Opay.core.Modules
{
    public class Transfer : ITransfer
    {
        private IConnectionClient _connectionClient;

        public Transfer(IConnectionClient connectionClient) {
        this._connectionClient = connectionClient;
    }

    public Task<JObject> transferToBank(SortedDictionary<String, Object> param) {
        return this._connectionClient.makePostRequestAsync(param,Endpoint.OPAY_TRANSFER_TOBANKS);
    }

    public Task<JObject> transferToWallet(SortedDictionary<String, Object> param) {
        return this._connectionClient.makePostRequestAsync(param,Endpoint.OPAY_TRANSFER_TOWALLET);
    }

    public Task<JObject> checkBankTransferStatus(SortedDictionary<String, Object> param) {
        return this._connectionClient.makePostRequestAsync(param,Endpoint.OPAY_TRANSFER_STATUS_TOBANKS);
    }

    public Task<JObject> checkWalletTransferStatus(SortedDictionary<String, Object> param) {
        return this._connectionClient.makePostRequestAsync(param,Endpoint.OPAY_TRANSFER_STATUS_TOWALLET);
    }

    public Task<JObject> allSupportingBanks(SortedDictionary<String, Object> param) {
        return this._connectionClient.makePostRequestAsync(param,Endpoint.OPAY_TRANSFER_SUPPORT_BANKS);
    }

    public Task<JObject> allSupportingCountries() {
        return this._connectionClient.makePostRequestAsync(Endpoint.OPAY_TRANSFER_SUPPORT_COUNTRIES);
    }
    }
}