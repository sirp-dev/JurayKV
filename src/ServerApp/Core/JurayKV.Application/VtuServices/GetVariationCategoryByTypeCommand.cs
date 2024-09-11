using JurayKV.Domain.Aggregates.CategoryVariationAggregate;
using JurayKV.Domain.Aggregates.VariationAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.VtuServices
{

     public sealed class GetVariationCategoryByTypeCommand : IRequest<List<CategoryVariation>>
    {
        public GetVariationCategoryByTypeCommand(VariationType categoryType)
        {
            CategoryType = categoryType;
        }

        public VariationType CategoryType { get; set; }

    }

    internal class GetVariationCategoryByTypeCommandHandler : IRequestHandler<GetVariationCategoryByTypeCommand, List<CategoryVariation>>
    {
        private readonly ICategoryVariationRepository _variationRepository;


        public GetVariationCategoryByTypeCommandHandler(ICategoryVariationRepository variationRepository)
        {
            _variationRepository = variationRepository;
        }

        public async Task<List<CategoryVariation>> Handle(GetVariationCategoryByTypeCommand request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));

            var result = await _variationRepository.GetByTypeAsync(request.CategoryType);
            return result;
        }
    }
}
