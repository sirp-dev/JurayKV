using JurayKV.Domain.Aggregates.VariationAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Domain.Aggregates.VariationAggregate
{
    public interface IVariationRepository
    {
        Task InsertAsync(Variation variation);

        Task UpdateAsync(Variation variation);

        Task DeleteAsync(Variation variation);
        Task<Variation> GetByIdAsync(Guid variationId);
        Task<List<Variation>> GetByCategoryIdAsync(Guid categoryId);
        Task<List<Variation>> GetByCategoryByActiveIdAsync(Guid categoryId);
        Task<List<Variation>> GetAllListAsync();
    }
}
