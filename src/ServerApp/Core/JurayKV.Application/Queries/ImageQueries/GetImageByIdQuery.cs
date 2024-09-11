using JurayKV.Application.Caching.Repositories;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.ImageQueries
{
    public sealed class GetImageByIdQuery : IRequest<ImageDto>
    {
        public GetImageByIdQuery(Guid id)
        {
            Id = id.ThrowIfEmpty(nameof(id));
        }

        public Guid Id { get; }

        // Handler
        private class GetImageByIdQueryHandler : IRequestHandler<GetImageByIdQuery, ImageDto>
        {
            private readonly IQueryRepository _repository;
            private readonly IImageCacheRepository _imageCacheRepository;

            public GetImageByIdQueryHandler(IQueryRepository repository, IImageCacheRepository imageCacheRepository)
            {
                _repository = repository;
                _imageCacheRepository = imageCacheRepository;
            }

            public async Task<ImageDto> Handle(GetImageByIdQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));


                ImageDto imageDetailsDto = await _imageCacheRepository.GetByIdAsync(request.Id);

                return imageDetailsDto;
            }
        }
    }

}
