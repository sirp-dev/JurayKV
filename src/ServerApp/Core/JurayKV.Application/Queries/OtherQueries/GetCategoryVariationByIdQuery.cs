using JurayKV.Domain.Aggregates.CategoryVariationAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.OtherQueries
{
    public sealed class GetCategoryVariationByIdQuery : IRequest<CategoryVariation>
    {
        public GetCategoryVariationByIdQuery(Guid id)
        {
            Id = id.ThrowIfEmpty(nameof(id));
        }

        public Guid Id { get; }

        // Handler
        public class GetCategoryVariationByIdQueryHandler : IRequestHandler<GetCategoryVariationByIdQuery, CategoryVariation>
        {
            private readonly ICategoryVariationRepository _categoryRepository;

            public GetCategoryVariationByIdQueryHandler(ICategoryVariationRepository categoryRepository)
            {
                _categoryRepository = categoryRepository;
            }

            public async Task<CategoryVariation> Handle(GetCategoryVariationByIdQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                CategoryVariation categoryVariation = await _categoryRepository.GetByIdAsync(request.Id);

                return categoryVariation;
            }
        }
    }

}
