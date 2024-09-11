using JurayKV.Application.Caching.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.AdvertRequestQueries
{
    public sealed class GetAdvertRequestListByCompanyIdQuery : IRequest<List<AdvertRequestListDto>>
    {
        public GetAdvertRequestListByCompanyIdQuery(Guid companyId)
        {
            CompanyId = companyId;
        }

        public Guid CompanyId { get; }
        private class GetAdvertRequestListByCompanyIdQueryHandler : IRequestHandler<GetAdvertRequestListByCompanyIdQuery, List<AdvertRequestListDto>>
        {
            private readonly IAdvertRequestCacheRepository _advertRequestCacheRepository;

            public GetAdvertRequestListByCompanyIdQueryHandler(IAdvertRequestCacheRepository advertRequestCacheRepository)
            {
                _advertRequestCacheRepository = advertRequestCacheRepository;
            }

            public async Task<List<AdvertRequestListDto>> Handle(GetAdvertRequestListByCompanyIdQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                List<AdvertRequestListDto> advertRequestDtos = await _advertRequestCacheRepository.GetListByCompanyAsync(request.CompanyId);
                return advertRequestDtos;
            }
        }
    }

}
