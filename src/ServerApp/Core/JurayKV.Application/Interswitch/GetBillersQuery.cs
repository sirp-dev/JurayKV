using JurayKvV.Infrastructure.Interswitch.Repositories;
using JurayKvV.Infrastructure.Interswitch.RequestModel;
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
    public sealed class GetBillersQuery : IRequest<BillerListResponse>
    {

    }

    internal class GetBillersQueryHandler : IRequestHandler<GetBillersQuery, BillerListResponse>
    {
        private readonly ISwitchRepository _switchRepository;

        public GetBillersQueryHandler(ISwitchRepository switchRepository)
        {
            _switchRepository = switchRepository;
        }

        public async Task<BillerListResponse> Handle(GetBillersQuery request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));

            BillerListResponse response = await _switchRepository.GetBillers();
            return response;
        }
    }
}
