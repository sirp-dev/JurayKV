using JurayKV.Domain.Aggregates.CategoryVariationAggregate;
using JurayKV.Domain.Aggregates.VariationAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.OtherQueries
{
    public sealed class GetVariationsByCategoryAndActiveQuery : IRequest<List<Variation>>
    {
        public GetVariationsByCategoryAndActiveQuery(Guid categoryId)
        {
            CategoryId = categoryId.ThrowIfEmpty(nameof(categoryId));
        }

        public Guid CategoryId { get; }

        // Handler
        public class GetVariationsByCategoryAndActiveQueryHandler : IRequestHandler<GetVariationsByCategoryAndActiveQuery, List<Variation>>
        {
            private readonly IVariationRepository _variationRepository;

            public GetVariationsByCategoryAndActiveQueryHandler(IVariationRepository variationRepository)
            {
                _variationRepository = variationRepository;
            }

            public async Task<List<Variation>> Handle(GetVariationsByCategoryAndActiveQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

               var variations = await _variationRepository.GetByCategoryByActiveIdAsync(request.CategoryId);

                return variations;
            }
        }
    }

}
