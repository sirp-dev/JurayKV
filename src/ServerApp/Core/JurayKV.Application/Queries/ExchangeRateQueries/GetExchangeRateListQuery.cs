using JurayKV.Application.Caching.Repositories;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.ExchangeRateQueries;

public sealed class GetExchangeRateListQuery : IRequest<List<ExchangeRateListDto>>
{
    private class GetExchangeRateListQueryHandler : IRequestHandler<GetExchangeRateListQuery, List<ExchangeRateListDto>>
    {
        private readonly IExchangeRateCacheRepository _exchangeRateCacheRepository;

        public GetExchangeRateListQueryHandler(IExchangeRateCacheRepository exchangeRateCacheRepository)
        {
            _exchangeRateCacheRepository = exchangeRateCacheRepository;
        }

        public async Task<List<ExchangeRateListDto>> Handle(GetExchangeRateListQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            List<ExchangeRateListDto> exchangeRateDtos = await _exchangeRateCacheRepository.GetListAsync();
            return exchangeRateDtos;
        }
    }
}
 