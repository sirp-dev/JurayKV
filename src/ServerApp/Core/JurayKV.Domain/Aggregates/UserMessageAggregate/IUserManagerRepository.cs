using JurayKV.Domain.Aggregates.GenericRepositoryInterface;
using JurayKV.Domain.Aggregates.SliderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.UserMessageAggregate
{
      public interface IUserMessageRepository : IGenericRepository<UserMessage>
    {
        Task<UserMessage> GetByIdAsync(Guid sliderId);

        Task InsertAsync(UserMessage slider);

        Task UpdateAsync(UserMessage slider);

        Task DeleteAsync(UserMessage slider);

        Task<List<UserMessage>> ListAllAsync ();
        Task<List<UserMessage>> ListAllByUserIdAsync (Guid userId); 
    }
}
