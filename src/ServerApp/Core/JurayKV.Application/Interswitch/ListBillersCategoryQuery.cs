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
   public sealed class ListBillersCategoryQuery : IRequest<BillerCategoryListResponse>
    {
        
    

    public class ListBillersCategoryQueryHandler : IRequestHandler<ListBillersCategoryQuery, BillerCategoryListResponse>
    {
        private readonly ISwitchRepository _switchRepository;

        public ListBillersCategoryQueryHandler(ISwitchRepository switchRepository)
        {
            _switchRepository = switchRepository;
        }

        public async Task<BillerCategoryListResponse> Handle(ListBillersCategoryQuery request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));

            var response = await _switchRepository.ListBillersCategory();
            return response;
        }
    }
    }
}
