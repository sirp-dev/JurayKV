using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Infrastructures;
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


    public sealed class CreateImageCommand : IRequest
    {
        public CreateImageCommand(ImageFile image, IFormFile file)
        {
            Image = image;
            File = file;
        }

        public ImageFile Image { get; }
        public IFormFile? File { get; set; }
    }

    internal class CreateImageCommandHandler : IRequestHandler<CreateImageCommand>
    {
        private readonly IImageRepository _imageRepository;
        private readonly IImageCacheHandler _imageCacheHandler;
        private readonly IStorageService _storage;

        public CreateImageCommandHandler(
                IImageRepository imageRepository,
                IImageCacheHandler imageCacheHandler,
                IStorageService storage)
        {
            _imageRepository = imageRepository;
            _imageCacheHandler = imageCacheHandler;
            _storage = storage;
        }

        public async Task Handle(CreateImageCommand request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));

            try
            {

                var xresult = await _storage.MainUploadFileReturnUrlAsync("", request.File);
                // 
                if (xresult.Message.Contains("200"))
                {
                    request.Image.ImageUrl = xresult.Url;
                    request.Image.ImageKey = xresult.Key;
                }

            }
            catch (Exception c)
            {

            }

            request.Image.CreatedAtUtc = DateTime.UtcNow;
            await _imageRepository.InsertAsync(request.Image);

            // Remove the cache
            await _imageCacheHandler.RemoveListAsync();

        }
    }
}
