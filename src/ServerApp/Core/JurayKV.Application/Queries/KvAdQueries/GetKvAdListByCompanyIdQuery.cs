using JurayKV.Application.Caching.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.KvAdQueries
{
     public sealed class GetKvAdListByCompanyIdQuery : IRequest<List<KvAdListDto>>
    {
        public GetKvAdListByCompanyIdQuery(Guid companyId)
        {
            CompanyId = companyId.ThrowIfEmpty(nameof(companyId));
        }

        public Guid CompanyId { get; }
        private class GetKvAdListByCompanyIdQueryHandler : IRequestHandler<GetKvAdListByCompanyIdQuery, List<KvAdListDto>>
        {
            private readonly IKvAdCacheRepository _kvAdCacheRepository;

            public GetKvAdListByCompanyIdQueryHandler(IKvAdCacheRepository kvAdCacheRepository)
            {
                _kvAdCacheRepository = kvAdCacheRepository;
            }

            public async Task<List<KvAdListDto>> Handle(GetKvAdListByCompanyIdQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                List<KvAdListDto> kvAdDtos = await _kvAdCacheRepository.GetListByCompanyIdAsync(request.CompanyId);
                return kvAdDtos;
            }
        }
    }

}
