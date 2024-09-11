using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Infrastructures;
using JurayKV.Application.Queries.SliderQueries;
using JurayKV.Domain.Aggregates.SliderAggregate;
using JurayKV.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.SliderCommands;

public sealed class UpdateSliderCommand : IRequest
{
    public UpdateSliderCommand(SliderDetailsDto slider, IFormFile fileone, IFormFile filetwo, bool removeImage)
    {
        Slider = slider;
        Fileone = fileone;
        Filetwo = filetwo;
        RemoveImage = removeImage;
    }

    public SliderDetailsDto Slider { get; }
    public IFormFile? Fileone { get; set; }
    public IFormFile? Filetwo { get; set; }
    public bool RemoveImage { get; set; }
}

internal class UpdateSliderCommandHandler : IRequestHandler<UpdateSliderCommand>
{
    private readonly ISliderRepository _sliderRepository;
    private readonly ISliderCacheHandler _sliderCacheHandler;
    private readonly IStorageService _storage;

    public UpdateSliderCommandHandler(
        ISliderRepository sliderRepository,
        ISliderCacheHandler sliderCacheHandler,
        IStorageService storage)
    {
        _sliderRepository = sliderRepository;
        _sliderCacheHandler = sliderCacheHandler;
        _storage = storage;
    }

    public async Task Handle(UpdateSliderCommand request, CancellationToken cancellationToken)
    {
        request.ThrowIfNull(nameof(request));
        var getupdate = await _sliderRepository.GetByIdAsync(request.Slider.Id);
        if (getupdate != null)
        {
            try
            {
                if (request.Fileone != null)
                {
                    var xresult = await _storage.MainUploadFileReturnUrlAsync(request.Slider.Key, request.Fileone);
                    // 
                    if (xresult.Message.Contains("200"))
                    {

                        getupdate.Url = xresult.Url;
                        getupdate.Key = xresult.Key;
                    }
                }

            }
            catch (Exception c)
            {

            }
            try
            {
                if (request.Filetwo != null)
                {
                    var xresult = await _storage.MainUploadFileReturnUrlAsync(request.Slider.SecondKey, request.Filetwo);
                    // 
                    if (xresult.Message.Contains("200"))
                    {

                        getupdate.SecondKey = xresult.Url;
                        getupdate.SecondUrl = xresult.Key;
                    }
                }

            }
            catch (Exception c)
            {

            }

            try
            {
                if (request.RemoveImage == true)
                {
                    await _storage.MainDeleteAsync(getupdate.SecondKey);
                }
            }
            catch (Exception c)
            {

            }

            getupdate.YoutubeVideo = request.Slider.YoutubeVideo;
            getupdate.IsVideo = request.Slider.IsVideo;
            getupdate.SortOrder = request.Slider.SortOrder;
            getupdate.Show = request.Slider.Show;
            getupdate.Title = request.Slider.Title;
            getupdate.MiniTitle = request.Slider.MiniTitle;
            getupdate.Text = request.Slider.Text;
            getupdate.ButtonLink = request.Slider.ButtonLink;
            getupdate.ButtonText = request.Slider.ButtonText;
            getupdate.DisableVideoButton = request.Slider.DisableVideoButton;
            getupdate.VideoLink = request.Slider.VideoLink;
            getupdate.VideoButtonText = request.Slider.VideoButtonText;



            await _sliderRepository.UpdateAsync(getupdate);

            await _sliderCacheHandler.RemoveListAsync();
            await _sliderCacheHandler.RemoveGetByIdAsync(getupdate.Id);
        }
    }
}
