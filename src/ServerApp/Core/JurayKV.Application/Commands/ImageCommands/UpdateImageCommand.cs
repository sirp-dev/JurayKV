using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Infrastructures;
using JurayKV.Application.Queries.ImageQueries;
using JurayKV.Domain.Aggregates.ImageAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.ImageCommands
{
    
public sealed class UpdateImageCommand : IRequest
    {
        public UpdateImageCommand(ImageDto image, IFormFile file)
        {
            Image = image;
            File = file;  
        }

        public ImageDto Image { get; }
        public IFormFile? File { get; set; } 
    }

    internal class UpdateImageCommandHandler : IRequestHandler<UpdateImageCommand>
    {
        private readonly IImageRepository _imageRepository;
        private readonly IImageCacheHandler _imageCacheHandler;
        private readonly IStorageService _storage;

        public UpdateImageCommandHandler(
            IImageRepository imageRepository,
            IImageCacheHandler imageCacheHandler,
            IStorageService storage)
        {
            _imageRepository = imageRepository;
            _imageCacheHandler = imageCacheHandler;
            _storage = storage;
        }

        public async Task Handle(UpdateImageCommand request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));
            var getupdate = await _imageRepository.GetByIdAsync(request.Image.Id);
            if (getupdate != null)
            {
                try
                {
                    if (request.File != null)
                    {
                        var xresult = await _storage.MainUploadFileReturnUrlAsync(request.Image.ImageKey, request.File);
                        // 
                        if (xresult.Message.Contains("200"))
                        {

                            getupdate.ImageUrl = xresult.Url;
                            getupdate.ImageKey = xresult.Key;
                        }
                    }

                }
                catch (Exception c)
                {

                }
            getupdate.ShowInDropdown = request.Image.ShowInDropdown;
                getupdate.Name = request.Image.Name;

                await _imageRepository.UpdateAsync(getupdate);

                await _imageCacheHandler.RemoveListAsync();
                await _imageCacheHandler.RemoveGetByIdAsync(getupdate.Id);
            }
        }
    }

}
