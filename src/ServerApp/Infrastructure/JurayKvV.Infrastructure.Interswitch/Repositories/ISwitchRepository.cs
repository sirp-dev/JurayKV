using JurayKvV.Infrastructure.Interswitch.RequestModel;
using JurayKvV.Infrastructure.Interswitch.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKvV.Infrastructure.Interswitch.Repositories
{
    public interface ISwitchRepository
    {
        
        Task<ComfirmationResponse> PaymentComfirmation (string merchantcode, string reference, string amount);

        #region
        Task<UssdIssuersResponse> GetUssdIssuers();
        Task<GenerateUssdResponse> GenerateUssdTransaction(string accessToken, UssdTransactionRequest requestData);
        Task<VirtualAccountTransactionResponse> CreateVirtualAccountTransaction(string accessToken, VirtualAccountTransactionRequest requestData);
        Task<VirtualAccountTransactionResponse> GetVirtualAccountTransaction(string accessToken, string merchantCode, string transactionReference);
        Task<PayableVirtualAccountResponse> CreatePayableVirtualAccount(string accessToken, PayableVirtualAccountRequest requestData);
        Task<QuickTellerServicesResponse> GetQuickTellerBillServices(string accessToken, string terminalId);
        Task<QuickTellerTransactionsResponse> QueryQuickTellerTransactions(string token, string requestRef, string terminalId);
        Task<QuickTellerServicesResponse> GetBillersByCategoryQuickTellerServices(string token, string categoryId, string terminalId);
        Task<QuickTellerServiceOptionsResponse> GetBillerPaymentItemQuickTellerServiceOptions(string token, string serviceId, string terminalId);
        Task<TransactionResponse> ProcessTransaction(string token, string terminalId, SendBillPaymentRequest transactionRequest);
        Task<ValidationResponse> ValidateCustomers(string token, string terminalId, ValidateCustomersRequest validateCustomersRequest);
        //Task<BillersCategoriesResponse> GetBillersCategories(string token, string terminalId);
        #endregion
        Task<BillerListResponse> GetBillers();
        Task<BillerCategoryListResponse> ListBillersCategory();
        Task<BillersByCategoryResponse> GetBillersByCategory(string categoryId);
        Task<BillerPaymentItemResponse> GetBillerPaymentItem(string billerId);
        Task<CustomerValidationResponse> CustomerValidation(string paymentCode, string customerId);

        Task<BankListResponse> BankList();
        Task<TransferFundResponse> SingleTransfer(TransferFundsRequest model);

        Task<AccountVerificationResponse> ValidateBankAccount(string accountNumber, string bankCode);
        Task<AccountVerificationResponse> NameInquiryByBVN(string bvn);

    }
}
