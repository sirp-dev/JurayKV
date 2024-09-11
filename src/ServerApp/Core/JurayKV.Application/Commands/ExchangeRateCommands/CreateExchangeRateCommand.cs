using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.ExchangeRateAggregate;
using JurayKV.Domain.ValueObjects;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.ExchangeRateCommands;

public sealed class CreateExchangeRateCommand : IRequest<Guid>
{
    public CreateExchangeRateCommand(decimal amount)
    {
        Amount = amount;
    }

    public decimal Amount { get; }

}

internal class CreateExchangeRateCommandHandler : IRequestHandler<CreateExchangeRateCommand, Guid>
{
    private readonly IExchangeRateRepository _exchangeRateRepository;
    private readonly IExchangeRateCacheHandler _exchangeRateCacheHandler;

    public CreateExchangeRateCommandHandler(
            IExchangeRateRepository exchangeRateRepository,
            IExchangeRateCacheHandler exchangeRateCacheHandler)
    {
        _exchangeRateRepository = exchangeRateRepository;
        _exchangeRateCacheHandler = exchangeRateCacheHandler;
    }

    public async Task<Guid> Handle(CreateExchangeRateCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));


        ExchangeRate exchangeRate = new ExchangeRate(Guid.NewGuid());
        exchangeRate.Amount = request.Amount;
        // Persist to the database
        await _exchangeRateRepository.InsertAsync(exchangeRate);

        // Remove the cache
        await _exchangeRateCacheHandler.RemoveListAsync();
        await _exchangeRateCacheHandler.RemoveGetAsync(exchangeRate.Id);
        await _exchangeRateCacheHandler.RemoveLastListAsync();
        await _exchangeRateCacheHandler.RemoveGetAsync(exchangeRate.Id);

        return exchangeRate.Id;
    }
}