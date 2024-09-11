using JurayKV.Domain.Aggregates.AdvertRequestAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.AdvertRequestQueries;

public sealed class IsAdvertRequestExistentByIdQuery : IRequest<bool>
{
    public IsAdvertRequestExistentByIdQuery(Guid departmetnId)
    {
        Id = departmetnId.ThrowIfEmpty(nameof(departmetnId));
    }

    public Guid Id { get; }

    private class IsAdvertRequestExistentByIdQueryHandler : IRequestHandler<IsAdvertRequestExistentByIdQuery, bool>
    {
        private readonly IQueryRepository _repository;

        public IsAdvertRequestExistentByIdQueryHandler(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(IsAdvertRequestExistentByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            bool isExists = await _repository.ExistsAsync<AdvertRequest>(d => d.Id == request.Id, cancellationToken);
            return isExists;
        }
    }
}
