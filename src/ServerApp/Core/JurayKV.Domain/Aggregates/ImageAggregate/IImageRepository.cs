using JurayKV.Domain.Aggregates.KvAdAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.ImageAggregate
{
    public interface IImageRepository
    {
        Task<ImageFile> GetByIdAsync(Guid imageId);
       
        Task InsertAsync(ImageFile imageFIle);

        Task UpdateAsync(ImageFile kvAd);

        Task DeleteAsync(ImageFile kvAd);

        Task<List<ImageFile>> ImageSHowInDropDown();
         
    }
}
