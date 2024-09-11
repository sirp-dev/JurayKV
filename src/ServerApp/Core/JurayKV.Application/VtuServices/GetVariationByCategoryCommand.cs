using JurayKV.Domain.Aggregates.VariationAggregate;
using JurayKV.Infrastructure.VTU.Repository;
using JurayKV.Infrastructure.VTU.RequestDto;
using JurayKV.Infrastructure.VTU.ResponseDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.VtuServices
{

    public sealed class GetVariationByCategoryCommand : IRequest<List<Variation>>
    {
        public GetVariationByCategoryCommand(Guid categoryId)
        {
           CategoryId = categoryId;

        }

        public Guid CategoryId { get; set; } 

    }

    internal class GetVariationByCategoryCommandHandler : IRequestHandler<GetVariationByCategoryCommand, List<Variation>>
    {
        private readonly IVariationRepository _variationRepository;


        public GetVariationByCategoryCommandHandler(IVariationRepository variationRepository)
        {
            _variationRepository = variationRepository;
        }

        public async Task<List<Variation>> Handle(GetVariationByCategoryCommand request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));
           
            var result = await _variationRepository.GetByCategoryByActiveIdAsync(request.CategoryId);
            return result;
        }
    }
}
