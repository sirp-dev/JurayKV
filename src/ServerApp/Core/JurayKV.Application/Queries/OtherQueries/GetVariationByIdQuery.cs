using Amazon.Runtime.Internal;
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
    public sealed class GetVariationByIdQuery : IRequest<Variation>
    {
        public GetVariationByIdQuery(Guid id)
        {
            Id = id.ThrowIfEmpty(nameof(id));
        }

        public Guid Id { get; }

        // Handler
        public class GetVariationByIdQueryHandler : IRequestHandler<GetVariationByIdQuery, Variation>
        {
            private readonly IVariationRepository _variationRepository;

            public GetVariationByIdQueryHandler(IVariationRepository variationRepository)
            {
                _variationRepository = variationRepository;
            }

            public async Task<Variation> Handle(GetVariationByIdQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                var variationData = await _variationRepository.GetByIdAsync(request.Id);
 
                return variationData;
            }
        }
    }

}
