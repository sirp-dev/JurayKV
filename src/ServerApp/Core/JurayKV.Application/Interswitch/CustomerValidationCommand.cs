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
    public sealed class CustomerValidationCommand : IRequest<CustomerValidationResponse>
    {
        public CustomerValidationCommand(string paymentCode, string customerId)
        {
            paymentCode = paymentCode;
            customerId = customerId;
        }

        public string paymentCode { get; }
        public string customerId { get; }
    }

    internal class CustomerValidationCommandHandler : IRequestHandler<CustomerValidationCommand, CustomerValidationResponse>
    {
        private readonly ISwitchRepository _switchRepository;

        public CustomerValidationCommandHandler(ISwitchRepository switchRepository)
        {
            _switchRepository = switchRepository;
        }

        public async Task<CustomerValidationResponse> Handle(CustomerValidationCommand request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));

            var response = await _switchRepository.CustomerValidation(request.paymentCode, request.customerId);
            return response;
        }
    }
}
