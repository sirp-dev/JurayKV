using JurayKV.Infrastructure.Flutterwave.Dtos;
using JurayKV.Infrastructure.Flutterwave.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Infrastructure.Flutterwave.Repositories
{
    public interface IFlutterTransactionService
    {
        Task<FlutterResponseDto> InitializeTransaction(PaymentRequestDto model);
        Task<InitializeTransactionTransferDto> InitializeTransactionTransfer(TransactionTransferInitialize model);

        Task<FlutterTransactionVerifyDto> VerifyTransaction(string tx_ref);
         Task<BillPaymentDto> PayBill(BillPaymentModel model);
    }
}
