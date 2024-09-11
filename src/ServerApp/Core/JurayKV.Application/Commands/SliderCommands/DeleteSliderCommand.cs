using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.SliderAggregate;
using JurayKV.Domain.Exceptions;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.SliderCommands;

public sealed class DeleteSliderCommand : IRequest
{
    public DeleteSliderCommand(Guid sliderId)
    {
        Id = sliderId.ThrowIfEmpty(nameof(sliderId));
    }

    public Guid Id { get; }
}

internal class DeleteSliderCommandHandler : IRequestHandler<DeleteSliderCommand>
{
    private readonly ISliderRepository _sliderRepository;
    private readonly ISliderCacheHandler _sliderCacheHandler;

    public DeleteSliderCommandHandler(ISliderRepository sliderRepository, ISliderCacheHandler sliderCacheHandler)
    {
        _sliderRepository = sliderRepository;
        _sliderCacheHandler = sliderCacheHandler;
    }

    public async Task Handle(DeleteSliderCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));

        Slider slider = await _sliderRepository.GetByIdAsync(request.Id);

        if (slider == null)
        {
            throw new EntityNotFoundException(typeof(Slider), request.Id);
        }

        await _sliderRepository.DeleteAsync(slider);
         
        await _sliderCacheHandler.RemoveListAsync(); 
        await _sliderCacheHandler.RemoveGetByIdAsync(slider.Id);
    }
}