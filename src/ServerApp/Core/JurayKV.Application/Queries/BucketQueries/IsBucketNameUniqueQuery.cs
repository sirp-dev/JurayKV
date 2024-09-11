using JurayKV.Domain.Aggregates.BucketAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.BucketQueries;

public sealed class IsBucketNameUniqueQuery : IRequest<bool>
{
    public IsBucketNameUniqueQuery(Guid departmentId, string name)
    {
        Id = departmentId.ThrowIfEmpty(nameof(departmentId));
        Name = name.ThrowIfNullOrEmpty(nameof(name));
    }

    public Guid Id { get; }

    public string Name { get; }

    private class IsBucketNameUniqueQueryHandler : IRequestHandler<IsBucketNameUniqueQuery, bool>
    {
        private readonly IQueryRepository _repository;

        public IsBucketNameUniqueQueryHandler(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(IsBucketNameUniqueQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            bool isExistent = await _repository.ExistsAsync<Bucket>(d => d.Id != request.Id && d.Name == request.Name, cancellationToken);
            return !isExistent;
        }
    }
}
