using JurayKV.Domain.Aggregates.CompanyAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.CompanyQueries;

public sealed class IsCompanyExistentByNameQuery : IRequest<bool>
{
    public IsCompanyExistentByNameQuery(string name)
    {
        Name = name.ThrowIfNullOrEmpty(nameof(name));
    }

    public string Name { get; set; }

    private class IsCompanyExistentByNameQueryHandler : IRequestHandler<IsCompanyExistentByNameQuery, bool>
    {
        private readonly IQueryRepository _repository;

        public IsCompanyExistentByNameQueryHandler(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(IsCompanyExistentByNameQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));
            bool isExists = await _repository.ExistsAsync<Company>(d => d.Name == request.Name, cancellationToken);
            return isExists;
        }
    }
}
