using JurayKV.Domain.Aggregates.NotificationAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.NotificationQueries;

public sealed class IsNotificationExistentByIdQuery : IRequest<bool>
{
    public IsNotificationExistentByIdQuery(Guid departmetnId)
    {
        Id = departmetnId.ThrowIfEmpty(nameof(departmetnId));
    }

    public Guid Id { get; }

    private class IsNotificationExistentByIdQueryHandler : IRequestHandler<IsNotificationExistentByIdQuery, bool>
    {
        private readonly IQueryRepository _repository;

        public IsNotificationExistentByIdQueryHandler(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(IsNotificationExistentByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            bool isExists = await _repository.ExistsAsync<Notification>(d => d.Id == request.Id, cancellationToken);
            return isExists;
        }
    }
}
