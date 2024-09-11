using JurayKV.Domain.Aggregates.CompanyAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.CompanyQueries;

public sealed class IsCompanyExistentByIdQuery : IRequest<bool>
{
    public IsCompanyExistentByIdQuery(Guid departmetnId)
    {
        Id = departmetnId.ThrowIfEmpty(nameof(departmetnId));
    }

    public Guid Id { get; }

    private class IsCompanyExistentByIdQueryHandler : IRequestHandler<IsCompanyExistentByIdQuery, bool>
    {
        private readonly IQueryRepository _repository;

        public IsCompanyExistentByIdQueryHandler(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(IsCompanyExistentByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            bool isExists = await _repository.ExistsAsync<Company>(d => d.Id == request.Id, cancellationToken);
            return isExists;
        }
    }
}
