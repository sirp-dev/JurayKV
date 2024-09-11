using JurayKV.Application.Caching.Repositories;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.CompanyQueries;

public sealed class GetCompanyDropdownListQuery : IRequest<List<CompanyDropdownListDto>>
{
    private class GetCompanyDropdownListQueryHandler : IRequestHandler<GetCompanyDropdownListQuery, List<CompanyDropdownListDto>>
    {
        private readonly ICompanyCacheRepository _companyCacheRepository;

        public GetCompanyDropdownListQueryHandler(ICompanyCacheRepository companyCacheRepository)
        {
            _companyCacheRepository = companyCacheRepository;
        }

        public async Task<List<CompanyDropdownListDto>> Handle(GetCompanyDropdownListQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            List<CompanyDropdownListDto> companyDtos = await _companyCacheRepository.GetDropdownListAsync();
            return companyDtos;
        }
    }
}

 