using JurayKV.Domain.Aggregates.CategoryVariationAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Application.VtuServices
{
    public sealed class GetVariationCategoryCommand : IRequest<List<CategoryVariation>>
    {
        public GetVariationCategoryCommand(BillGateway billGateway)
        {
            BillGateway = billGateway;
        }
        public BillGateway BillGateway { get; set; }
    

    public class GetVariationCategoryCommandHandler : IRequestHandler<GetVariationCategoryCommand, List<CategoryVariation>>
    {
        private readonly ICategoryVariationRepository _variationRepository;


        public GetVariationCategoryCommandHandler(ICategoryVariationRepository variationRepository)
        {
            _variationRepository = variationRepository;
        }

        public async Task<List<CategoryVariation>> Handle(GetVariationCategoryCommand request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));

            var result = await _variationRepository.GetAllListByBillerAsync(request.BillGateway);
            return result;
        }
    }
}
}
