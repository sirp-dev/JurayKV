using JurayKV.Domain.Aggregates.ExchangeRateAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.ExchangeRateQueries;

public sealed class IsExchangeRateExistentByIdQuery : IRequest<bool>
{
    public IsExchangeRateExistentByIdQuery(Guid departmetnId)
    {
        Id = departmetnId.ThrowIfEmpty(nameof(departmetnId));
    }

    public Guid Id { get; }

    private class IsExchangeRateExistentByIdQueryHandler : IRequestHandler<IsExchangeRateExistentByIdQuery, bool>
    {
        private readonly IQueryRepository _repository;

        public IsExchangeRateExistentByIdQueryHandler(IQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(IsExchangeRateExistentByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            bool isExists = await _repository.ExistsAsync<ExchangeRate>(d => d.Id == request.Id, cancellationToken);
            return isExists;
        }
    }
}
