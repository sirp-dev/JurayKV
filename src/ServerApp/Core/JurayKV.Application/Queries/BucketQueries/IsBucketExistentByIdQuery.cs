using JurayKV.Domain.Aggregates.BucketAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.BucketQueries;

public sealed class IsBucketExistentByIdQuery : IRequest<bool>
{
    public IsBucketExistentByIdQuery(Guid departmetnId)
    {
        Id = departmetnId.ThrowIfEmpty(nameof(departmetnId));
    }

    public Guid Id { get; }

    private class IsBucketExistentByIdQueryHandler : IRequestHandler<IsBucketExistentByIdQuery, bool>
    {
        private readonly IQueryRepository _repository;

        public IsBucketExistentByIdQueryHandler(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(IsBucketExistentByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            bool isExists = await _repository.ExistsAsync<Bucket>(d => d.Id == request.Id, cancellationToken);
            return isExists;
        }
    }
}
