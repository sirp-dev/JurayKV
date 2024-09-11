using JurayKV.Infrastructure.VTU.Repository;
using JurayKV.Infrastructure.VTU.RequestDto;
using JurayKV.Infrastructure.VTU.ResponseDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.VtuServices
{
    public sealed class VerifyCustomerCommand : IRequest<VerifyResponse>
    {
        public VerifyCustomerCommand(string customerid, string serviceid, string variationid)
        {
            CustomerId = customerid;
            ServiceId = serviceid;
            VariationId = variationid;

        }

        public string CustomerId { get; set; }
        public string ServiceId { get; set; }
        public string VariationId { get; set; }

    }

    internal class VerifyCustomerCommandHandler : IRequestHandler<VerifyCustomerCommand, VerifyResponse>
    {
        private readonly IVtuService _vtuService;


        public VerifyCustomerCommandHandler(

                IVtuService vtuService)
        {
            _vtuService = vtuService;
        }

        public async Task<VerifyResponse> Handle(VerifyCustomerCommand request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));
            VerifyRequest data = new VerifyRequest();
            data.CustomerId = request.CustomerId;
            data.ServiceId = request.ServiceId;
            data.VariationId = request.VariationId;
            var result = await _vtuService.VerifyCustomerUtility(data);
            return result;
        }
    }
}
