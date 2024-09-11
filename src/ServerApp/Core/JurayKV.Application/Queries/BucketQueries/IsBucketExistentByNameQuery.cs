using JurayKV.Domain.Aggregates.BucketAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.BucketQueries;

public sealed class IsBucketExistentByNameQuery : IRequest<bool>
{
    public IsBucketExistentByNameQuery(string name)
    {
        Name = name.ThrowIfNullOrEmpty(nameof(name));
    }

    public string Name { get; set; }

    private class IsBucketExistentByNameQueryHandler : IRequestHandler<IsBucketExistentByNameQuery, bool>
    {
        private readonly IQueryRepository _repository;

        public IsBucketExistentByNameQueryHandler(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(IsBucketExistentByNameQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));
            bool isExists = await _repository.ExistsAsync<Bucket>(d => d.Name == request.Name, cancellationToken);
            return isExists;
        }
    }
}
