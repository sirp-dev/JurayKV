using JurayKV.Application.Caching.Repositories;
using JurayKV.Domain.Aggregates.CompanyAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.CompanyQueries
{
    public sealed class GetCompanyByUserIdQuery : IRequest<CompanyDetailsDto>
    {
        public GetCompanyByUserIdQuery(Guid id)
        {
            Id = id.ThrowIfEmpty(nameof(id));
        }

        public Guid Id { get; }

        // Handler
        private class GetCompanyByUserIdQueryHandler : IRequestHandler<GetCompanyByUserIdQuery, CompanyDetailsDto>
        {
            private readonly ICompanyCacheRepository _companyCacheRepository;

            public GetCompanyByUserIdQueryHandler(ICompanyCacheRepository companyCacheRepository)
            {
                _companyCacheRepository = companyCacheRepository;
            }

            public async Task<CompanyDetailsDto> Handle(GetCompanyByUserIdQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                Expression<Func<Company, CompanyDetailsDto>> selectExp = d => new CompanyDetailsDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    CreatedAtUtc = d.CreatedAtUtc,
                    AmountPerPoint = d.AmountPerPoint,
                };

                CompanyDetailsDto companyDetailsDto = await _companyCacheRepository.GetByUserIdAsync(request.Id);

                return companyDetailsDto;
            }
        }
    }

}
