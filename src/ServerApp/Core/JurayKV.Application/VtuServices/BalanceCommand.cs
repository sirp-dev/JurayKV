using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.BucketAggregate;
using JurayKV.Infrastructure.VTU.Repository;
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
    public sealed class BalanceCommand : IRequest<BalanceResponse>
    {
    }

    internal class BalanceCommandHandler : IRequestHandler<BalanceCommand, BalanceResponse>
    {
        private readonly IVtuService _vtuService;


        public BalanceCommandHandler(

                IVtuService vtuService)
        {
            _vtuService = vtuService;
        }

        public async Task<BalanceResponse> Handle(BalanceCommand request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));
            var bal = await _vtuService.Balance();
            return bal;
        }
    }
}
