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
     public sealed class BankListQuery : IRequest<BankListResponse>
    {

    }

    internal class BankListQueryHandler : IRequestHandler<BankListQuery, BankListResponse>
    {
        private readonly ISwitchRepository _switchRepository;

        public BankListQueryHandler(ISwitchRepository switchRepository)
        {
            _switchRepository = switchRepository;
        }

        public async Task<BankListResponse> Handle(BankListQuery request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));

            BankListResponse response = await _switchRepository.BankList();
            return response;
        }
    }
}
