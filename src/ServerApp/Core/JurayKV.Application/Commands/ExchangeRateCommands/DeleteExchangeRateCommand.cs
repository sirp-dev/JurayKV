using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.ExchangeRateAggregate;
using JurayKV.Domain.Exceptions;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.ExchangeRateCommands;

public sealed class DeleteExchangeRateCommand : IRequest
{
    public DeleteExchangeRateCommand(Guid exchangeRateId)
    {
        Id = exchangeRateId.ThrowIfEmpty(nameof(exchangeRateId));
    }

    public Guid Id { get; }
}

internal class DeleteExchangeRateCommandHandler : IRequestHandler<DeleteExchangeRateCommand>
{
    private readonly IExchangeRateRepository _exchangeRateRepository;
    private readonly IExchangeRateCacheHandler _exchangeRateCacheHandler;

    public DeleteExchangeRateCommandHandler(IExchangeRateRepository exchangeRateRepository, IExchangeRateCacheHandler exchangeRateCacheHandler)
    {
        _exchangeRateRepository = exchangeRateRepository;
        _exchangeRateCacheHandler = exchangeRateCacheHandler;
    }

    public async Task Handle(DeleteExchangeRateCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));

        ExchangeRate exchangeRate = await _exchangeRateRepository.GetByIdAsync(request.Id);

        if (exchangeRate == null)
        {
            throw new EntityNotFoundException(typeof(ExchangeRate), request.Id);
        }

        await _exchangeRateRepository.DeleteAsync(exchangeRate);
        // Remove the cache
        await _exchangeRateCacheHandler.RemoveListAsync();
        await _exchangeRateCacheHandler.RemoveGetAsync(exchangeRate.Id);
        await _exchangeRateCacheHandler.RemoveLastListAsync();
        await _exchangeRateCacheHandler.RemoveGetAsync(exchangeRate.Id);
    }
}