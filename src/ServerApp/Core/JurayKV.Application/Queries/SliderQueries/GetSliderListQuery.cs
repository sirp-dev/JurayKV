using JurayKV.Application.Caching.Repositories;
using MediatR;
using System.Linq;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.SliderQueries;

public sealed class GetSliderListQuery : IRequest<List<SliderDetailsDto>>
{
    private class GetSliderListQueryHandler : IRequestHandler<GetSliderListQuery, List<SliderDetailsDto>>
    {
        private readonly ISliderCacheRepository _sliderCacheRepository;

        public GetSliderListQueryHandler(ISliderCacheRepository sliderCacheRepository)
        {
            _sliderCacheRepository = sliderCacheRepository;
        }

        public async Task<List<SliderDetailsDto>> Handle(GetSliderListQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            List<SliderDetailsDto> sliderDtos = await _sliderCacheRepository.GetListAsync();
            return sliderDtos.Where(x=>x.Show == true).ToList();
        }
    }
}
 