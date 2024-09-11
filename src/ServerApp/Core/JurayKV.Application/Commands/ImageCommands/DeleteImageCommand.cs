using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.ImageAggregate;
using JurayKV.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.ImageCommands
{

    public sealed class DeleteImageCommand : IRequest
    {
        public DeleteImageCommand(Guid imageId)
        {
            Id = imageId.ThrowIfEmpty(nameof(imageId));
        }

        public Guid Id { get; }
    }

    internal class DeleteImageCommandHandler : IRequestHandler<DeleteImageCommand>
    {
        private readonly IImageRepository _imageRepository;
        private readonly IImageCacheHandler _imageCacheHandler;

        public DeleteImageCommandHandler(IImageRepository imageRepository, IImageCacheHandler imageCacheHandler)
        {
            _imageRepository = imageRepository;
            _imageCacheHandler = imageCacheHandler;
        }

        public async Task Handle(DeleteImageCommand request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));

            ImageFile image = await _imageRepository.GetByIdAsync(request.Id);

            if (image == null)
            {
                throw new EntityNotFoundException(typeof(ImageFile), request.Id);
            }

            await _imageRepository.DeleteAsync(image);

            await _imageCacheHandler.RemoveListAsync();
            await _imageCacheHandler.RemoveGetByIdAsync(image.Id);
        }
    }

}