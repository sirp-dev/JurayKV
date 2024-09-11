using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.BucketAggregate;
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
    public sealed class PaymentComfirmationCommand : IRequest<ComfirmationResponse>
    {
        public PaymentComfirmationCommand(string merchantcode, string reference, string amount)
        {
            Merchantcode = merchantcode;
            Reference = reference;
            Amount = amount;
        }

        public string Merchantcode { get; }
        public string Reference { get; }
        public string Amount { get; }
    }

    internal class PaymentComfirmationCommandHandler : IRequestHandler<PaymentComfirmationCommand, ComfirmationResponse>
    {
        private readonly ISwitchRepository _switchRepository;

        public PaymentComfirmationCommandHandler(ISwitchRepository switchRepository)
        {
            _switchRepository = switchRepository;
        }

        public async Task<ComfirmationResponse> Handle(PaymentComfirmationCommand request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));

            var response = await _switchRepository.PaymentComfirmation(request.Merchantcode, request.Reference, request.Amount);
            return response;
        }
    }
}
