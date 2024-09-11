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
    public sealed class SingleTransferCommand : IRequest<TransferFundResponse>
    {
        public SingleTransferCommand(TransferFundsRequest model)
        {
            ModelData = model;
        }

        public TransferFundsRequest ModelData { get; }
    }

    internal class SingleTransferCommandHandler : IRequestHandler<SingleTransferCommand, TransferFundResponse>
    {
        private readonly ISwitchRepository _switchRepository;

        public SingleTransferCommandHandler(ISwitchRepository switchRepository)
        {
            _switchRepository = switchRepository;
        }

        public async Task<TransferFundResponse> Handle(SingleTransferCommand request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));

            TransferFundResponse response = await _switchRepository.SingleTransfer(request.ModelData);
            return response;
        }
    }
}
