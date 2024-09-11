using JurayKV.Infrastructure.Opay.core.Request;
using JurayKV.Infrastructure.Opay.core.Response;
using Newtonsoft.Json.Linq;

namespace JurayKV.Infrastructure.Opay.core.Repositories
{
    public interface IOpayRepository
    {
        Task<TransactionResponse> initializeTransaction(TransactionRequest model);
        Task<TransactionStatusResponse> transactionStatus(TransactionStatusRequest model);
        Task WebhookResquest(WebhookResponse model);
        Task<BankListResponse> getBankList();
        Task<VerifyAccountResponse> verifyBankAccount(VerifyAccountRequest model);
        Task<TransferToBankResponse> transferToBank(TransferToBankRequest model);
    }
}
