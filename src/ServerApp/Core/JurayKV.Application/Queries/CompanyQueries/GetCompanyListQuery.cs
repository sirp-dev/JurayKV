using JurayKV.Application.Caching.Repositories;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.CompanyQueries;

public sealed class GetCompanyListQuery : IRequest<List<CompanyListDto>>
{
    private class GetCompanyListQueryHandler : IRequestHandler<GetCompanyListQuery, List<CompanyListDto>>
    {
        private readonly ICompanyCacheRepository _companyCacheRepository;

        public GetCompanyListQueryHandler(ICompanyCacheRepository companyCacheRepository)
        {
            _companyCacheRepository = companyCacheRepository;
        }

        public async Task<List<CompanyListDto>> Handle(GetCompanyListQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            List<CompanyListDto> companyDtos = await _companyCacheRepository.GetListAsync();
            return companyDtos;
        }
    }
}

 