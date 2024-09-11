using JurayKV.Domain.Aggregates.AdvertRequestAggregate;
using JurayKV.Infrastructure.Flutterwave.Dtos;
using JurayKV.Infrastructure.Flutterwave.Models;
using JurayKV.Infrastructure.Flutterwave.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Flutterwaves
{
    public sealed class CreateBillTransactionQuery : IRequest<BillPaymentDto>
    {
        public CreateBillTransactionQuery(BillPaymentModel data)
        {
            Data = data;
        }

        public BillPaymentModel Data { get; set; }



        public class CreateBillTransactionQueryHandler : IRequestHandler<CreateBillTransactionQuery, BillPaymentDto>
        {
            private readonly IFlutterTransactionService _repositoryService;

            public CreateBillTransactionQueryHandler(IFlutterTransactionService repositoryService)
            {
                _repositoryService = repositoryService ?? throw new ArgumentNullException(nameof(repositoryService));
            }

            public async Task<BillPaymentDto> Handle(CreateBillTransactionQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));


                BillPaymentModel model = new BillPaymentModel();
                model.country = request.Data.country;
                model.customer = request.Data.country;
                model.amount = request.Data.amount;
                model.recurrence = request.Data.recurrence;
                model.type = request.Data.type;
                model.reference = request.Data.reference;
                model.biller_name = request.Data.biller_name;


                BillPaymentDto repositories = await _repositoryService.PayBill(model);

                return repositories;
            }

        }
    }

}
