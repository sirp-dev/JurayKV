using System.Linq.Expressions;
using JurayKV.Application.Caching.Repositories;
using JurayKV.Domain.Aggregates.SliderAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Queries.SliderQueries;

public sealed class GetSliderByIdQuery : IRequest<SliderDetailsDto>
{
    public GetSliderByIdQuery(Guid id)
    {
        Id = id.ThrowIfEmpty(nameof(id));
    }

    public Guid Id { get; }

    // Handler
    private class GetSliderByIdQueryHandler : IRequestHandler<GetSliderByIdQuery, SliderDetailsDto>
    {
        private readonly IQueryRepository _repository;
        private readonly ISliderCacheRepository _sliderCacheRepository;

        public GetSliderByIdQueryHandler(IQueryRepository repository, ISliderCacheRepository sliderCacheRepository)
        {
            _repository = repository;
            _sliderCacheRepository = sliderCacheRepository;
        }

        public async Task<SliderDetailsDto> Handle(GetSliderByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));
 
             
            SliderDetailsDto sliderDetailsDto = await _sliderCacheRepository.GetByIdAsync(request.Id);

            return sliderDetailsDto;
        }
    }
}
