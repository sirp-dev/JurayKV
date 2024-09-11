using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Infrastructures;
using JurayKV.Domain.Aggregates.SliderAggregate;
using JurayKV.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.SliderCommands;

public sealed class CreateSliderCommand : IRequest
{
    public CreateSliderCommand(Slider slider, IFormFile fileone, IFormFile filetwo)
    {
        Slider = slider;
        Fileone = fileone;
        Filetwo = filetwo;
    }

    public Slider Slider { get; }
    public IFormFile? Fileone {  get; set; }
    public IFormFile? Filetwo {  get; set; }
}

internal class CreateSliderCommandHandler : IRequestHandler<CreateSliderCommand>
{
    private readonly ISliderRepository _sliderRepository;
    private readonly ISliderCacheHandler _sliderCacheHandler;
    private readonly IStorageService _storage;

    public CreateSliderCommandHandler(
            ISliderRepository sliderRepository,
            ISliderCacheHandler sliderCacheHandler,
            IStorageService storage)
    {
        _sliderRepository = sliderRepository;
        _sliderCacheHandler = sliderCacheHandler;
        _storage = storage;
    }

    public async Task Handle(CreateSliderCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));
         
        try
        {

            var xresult = await _storage.MainUploadFileReturnUrlAsync("", request.Fileone);
            // 
            if (xresult.Message.Contains("200"))
            {
                request.Slider.Url = xresult.Url;
                request.Slider.Key = xresult.Key;
            }

        }
        catch (Exception c)
        {

        }
        try
        {

            var xresult = await _storage.MainUploadFileReturnUrlAsync("", request.Filetwo);
            // 
            if (xresult.Message.Contains("200"))
            {
                request.Slider.SecondUrl = xresult.Url;
                request.Slider.SecondKey = xresult.Key;
            }

        }
        catch (Exception c)
        {

        }



        await _sliderRepository.Create(request.Slider);

        // Remove the cache
        await _sliderCacheHandler.RemoveListAsync();

    }
}