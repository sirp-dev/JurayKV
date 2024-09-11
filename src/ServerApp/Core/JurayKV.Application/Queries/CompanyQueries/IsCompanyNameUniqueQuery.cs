using JurayKV.Domain.Aggregates.CompanyAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.CompanyQueries;

public sealed class IsCompanyNameUniqueQuery : IRequest<bool>
{
    public IsCompanyNameUniqueQuery(Guid companyId, string name)
    {
        Id = companyId.ThrowIfEmpty(nameof(companyId));
        Name = name.ThrowIfNullOrEmpty(nameof(name));
    }

    public Guid Id { get; }

    public string Name { get; }

    private class IsCompanyNameUniqueQueryHandler : IRequestHandler<IsCompanyNameUniqueQuery, bool>
    {
        private readonly IQueryRepository _repository;

        public IsCompanyNameUniqueQueryHandler(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(IsCompanyNameUniqueQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            bool isExistent = await _repository.ExistsAsync<Company>(d => d.Id != request.Id && d.Name == request.Name, cancellationToken);
            return !isExistent;
        }
    }
}
