using System.Linq.Expressions;
using JurayKV.Application.Caching.Repositories;
using JurayKV.Domain.Aggregates.CompanyAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.CompanyQueries;

public sealed class GetCompanyByIdQuery : IRequest<CompanyDetailsDto>
{
    public GetCompanyByIdQuery(Guid id)
    {
        Id = id.ThrowIfEmpty(nameof(id));
    }

    public Guid Id { get; }

    // Handler
    private class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, CompanyDetailsDto>
    {
        private readonly ICompanyCacheRepository _companyCacheRepository;

        public GetCompanyByIdQueryHandler(ICompanyCacheRepository companyCacheRepository)
        {
            _companyCacheRepository = companyCacheRepository;
        }

        public async Task<CompanyDetailsDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            Expression<Func<Company, CompanyDetailsDto>> selectExp = d => new CompanyDetailsDto
            {
                Id = d.Id,
               Name= d.Name,
               CreatedAtUtc = d.CreatedAtUtc,
               AmountPerPoint = d.AmountPerPoint,
            };

            CompanyDetailsDto companyDetailsDto = await _companyCacheRepository.GetByIdAsync(request.Id);

            return companyDetailsDto;
        }
    }
}

 