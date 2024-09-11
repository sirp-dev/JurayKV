using JurayKV.Domain.Aggregates.CategoryVariationAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Domain.Aggregates.CategoryVariationAggregate
{
    public interface ICategoryVariationRepository
    {

        Task InsertAsync(CategoryVariation categoryVariation);

        Task UpdateAsync(CategoryVariation categoryVariation);

        Task DeleteAsync(CategoryVariation categoryVariation);
        Task<CategoryVariation> GetByIdAsync(Guid categoryVariationId);

        Task<List<CategoryVariation>> GetByTypeAsync(VariationType variationType);
        Task<List<CategoryVariation>> GetByTypeByActiveAsync(Domain.Primitives.Enum.VariationType variationType);
        Task<List<CategoryVariation>> GetAllListAsync();
        Task<List<CategoryVariation>> GetAllListByBillerAsync(BillGateway biller);
    }
}
