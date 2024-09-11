using JurayKvV.Infrastructure.Interswitch.Repositories;
using JurayKvV.Infrastructure.Interswitch.ResponseModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Interswitch
{
      public sealed class GetBillerPaymentItemQuery : IRequest<BillerPaymentItemResponse>
    {
        public GetBillerPaymentItemQuery(string billerId)
        {
            BillerId = billerId;
        }

        public string BillerId { get; }
    }

    internal class GetBillerPaymentItemQueryHandler : IRequestHandler<GetBillerPaymentItemQuery, BillerPaymentItemResponse>
    {
        private readonly ISwitchRepository _switchRepository;

        public GetBillerPaymentItemQueryHandler(ISwitchRepository switchRepository)
        {
            _switchRepository = switchRepository;
        }

        public async Task<BillerPaymentItemResponse> Handle(GetBillerPaymentItemQuery request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));

            var response = await _switchRepository.GetBillerPaymentItem(request.BillerId);
            return response;
        }
    }
}
