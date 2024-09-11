using JurayKV.Domain.Aggregates.GenericRepositoryInterface;
using JurayKV.Domain.Aggregates.SliderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.StateLgaAggregate
{
    public interface IStateRepository : IGenericRepository<State>
    {
        Task<List<State>> GetAllState();

    }

    public interface ILgaRepository : IGenericRepository<LocalGoverment>
    {
        Task<List<LocalGoverment>> GetAllLGA(Guid stateId);

    }
}
