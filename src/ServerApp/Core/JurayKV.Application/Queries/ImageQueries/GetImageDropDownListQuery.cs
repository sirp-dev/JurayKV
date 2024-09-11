using JurayKV.Application.Caching.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.ImageQueries
{
     public sealed class GetImageDropDownListQuery : IRequest<List<ImageDto>>
    {
        private class GetImageDropDownListQueryHandler : IRequestHandler<GetImageDropDownListQuery, List<ImageDto>>
        {
            private readonly IImageCacheRepository _imageCacheRepository;

            public GetImageDropDownListQueryHandler(IImageCacheRepository imageCacheRepository)
            {
                _imageCacheRepository = imageCacheRepository;
            }

            public async Task<List<ImageDto>> Handle(GetImageDropDownListQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                List<ImageDto> imageDtos = await _imageCacheRepository.GetListDropdownAsync();
                return imageDtos;
            }
        }
    }

}
