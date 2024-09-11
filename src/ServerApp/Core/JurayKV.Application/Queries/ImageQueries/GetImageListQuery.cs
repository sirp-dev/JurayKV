using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Queries.ImageQueries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.ImageQueries
{
     public sealed class GetImageListQuery : IRequest<List<ImageDto>>
    {
        private class GetImageListQueryHandler : IRequestHandler<GetImageListQuery, List<ImageDto>>
        {
            private readonly IImageCacheRepository _imageCacheRepository;

            public GetImageListQueryHandler(IImageCacheRepository imageCacheRepository)
            {
                _imageCacheRepository = imageCacheRepository;
            }

            public async Task<List<ImageDto>> Handle(GetImageListQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                List<ImageDto> imageDtos = await _imageCacheRepository.GetListAsync();
                return imageDtos.OrderByDescending(x=>x.CreatedAtUtc).ToList();
            }
        }
    }

}
