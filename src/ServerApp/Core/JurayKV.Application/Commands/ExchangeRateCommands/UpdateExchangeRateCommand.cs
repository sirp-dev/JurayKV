using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.ExchangeRateAggregate;
using JurayKV.Domain.Exceptions;
using JurayKV.Domain.ValueObjects;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.ExchangeRateCommands;

public sealed class UpdateExchangeRateCommand : IRequest
{
    public UpdateExchangeRateCommand(
        Guid id,
        decimal amount)
    {
        Id = id;
       Amount = amount;
    }

    public Guid Id { get; }

    public decimal Amount { get; }


 }

internal class UpdateExchangeRateCommandHandler : IRequestHandler<UpdateExchangeRateCommand>
{
    private readonly IExchangeRateRepository _exchangeRateRepository;
    private readonly IExchangeRateCacheHandler _exchangeRateCacheHandler;

    public UpdateExchangeRateCommandHandler(
        IExchangeRateRepository exchangeRateRepository,
        IExchangeRateCacheHandler exchangeRateCacheHandler)
    {
        _exchangeRateRepository = exchangeRateRepository;
        _exchangeRateCacheHandler = exchangeRateCacheHandler;
    }

    public async Task Handle(UpdateExchangeRateCommand request, CancellationToken cancellationToken)
    {
        request.ThrowIfNull(nameof(request));

        ExchangeRate exchangeRateToBeUpdated = await _exchangeRateRepository.GetByIdAsync(request.Id);

        if (exchangeRateToBeUpdated == null)
        {
            throw new EntityNotFoundException(typeof(ExchangeRate), request.Id);
        }
         
        exchangeRateToBeUpdated.Amount = request.Amount;

        await _exchangeRateRepository.UpdateAsync(exchangeRateToBeUpdated);

        // Remove the cache
        await _exchangeRateCacheHandler.RemoveListAsync();
        await _exchangeRateCacheHandler.RemoveGetAsync(exchangeRateToBeUpdated.Id);
        await _exchangeRateCacheHandler.RemoveLastListAsync();
        await _exchangeRateCacheHandler.RemoveGetAsync(exchangeRateToBeUpdated.Id);

    }
}
