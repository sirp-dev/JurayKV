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
     public sealed class GetBillersByCategoryQuery : IRequest<BillersByCategoryResponse>
    {
        public GetBillersByCategoryQuery(string categoryId)
        {
            CategoryId = categoryId; 
        }

        public string CategoryId { get; } 
    }

    internal class GetBillersByCategoryQueryHandler : IRequestHandler<GetBillersByCategoryQuery, BillersByCategoryResponse>
    {
        private readonly ISwitchRepository _switchRepository;

        public GetBillersByCategoryQueryHandler(ISwitchRepository switchRepository)
        {
            _switchRepository = switchRepository;
        }

        public async Task<BillersByCategoryResponse> Handle(GetBillersByCategoryQuery request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));

            var response = await _switchRepository.GetBillersByCategory(request.CategoryId);
            return response;
        }
    }
}
