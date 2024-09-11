using JurayKV.Domain.Aggregates.BucketAggregate;
using JurayKV.Domain.Aggregates.GenericRepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.SliderAggregate
{
    public interface ISliderRepository : IGenericRepository<Slider>
    {
        Task<Slider> GetByIdAsync(Guid sliderId);

        Task InsertAsync(Slider slider);

        Task UpdateAsync(Slider slider);

        Task DeleteAsync(Slider slider);
    }
    
}
