using System.Linq.Expressions;
using JurayKV.Application.Caching.Repositories;
using JurayKV.Domain.Aggregates.ExchangeRateAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.ExchangeRateQueries;

public sealed class GetExchangeRateByIdQuery : IRequest<ExchangeRateDetailsDto>
{
    public GetExchangeRateByIdQuery(Guid id)
    {
        Id = id.ThrowIfEmpty(nameof(id));
    }

    public Guid Id { get; }

    // Handler
    private class GetExchangeRateByIdQueryHandler : IRequestHandler<GetExchangeRateByIdQuery, ExchangeRateDetailsDto>
    {
        private readonly IExchangeRateCacheRepository _exchangeRateCacheRepository;

        public GetExchangeRateByIdQueryHandler(IQueryRepository repository, IExchangeRateCacheRepository exchangeRateCacheRepository)
        {
            _exchangeRateCacheRepository = exchangeRateCacheRepository;
        }

        public async Task<ExchangeRateDetailsDto> Handle(GetExchangeRateByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            Expression<Func<ExchangeRate, ExchangeRateDetailsDto>> selectExp = d => new ExchangeRateDetailsDto
            {
                Id = d.Id,
                Amount = d.Amount,
                CreatedAtUtc = d.CreatedAtUtc,
            };

            ExchangeRateDetailsDto exchangeRateDetailsDto = await _exchangeRateCacheRepository.GetByIdAsync(request.Id);

            return exchangeRateDetailsDto;
        }
    }
}
 