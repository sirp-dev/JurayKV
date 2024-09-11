using JurayKV.Application.Queries.SliderQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace JurayKV.Application.Caching.Repositories
{
    [ScopedService]
    public interface ISliderCacheRepository
    {
        Task<List<SliderDetailsDto>> GetListAsync();
        
        Task<SliderDetailsDto> GetByIdAsync(Guid modelId); 
    }
     
}
