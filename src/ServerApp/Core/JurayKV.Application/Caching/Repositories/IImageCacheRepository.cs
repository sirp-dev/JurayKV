using JurayKV.Application.Queries.ImageQueries;
using JurayKV.Application.Queries.WalletQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace JurayKV.Application.Caching.Repositories
{
       [ScopedService]
    public interface IImageCacheRepository
    {
        Task<List<ImageDto>> GetListAsync();
        Task<List<ImageDto>> GetListDropdownAsync();

        Task<ImageDto> GetByIdAsync(Guid modelId);
        
    }
}
