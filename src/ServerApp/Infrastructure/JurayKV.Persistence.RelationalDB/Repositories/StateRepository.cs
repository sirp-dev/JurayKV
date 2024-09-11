using JurayKV.Domain.Aggregates.BucketAggregate;
using JurayKV.Domain.Aggregates.GenericRepositoryInterface;
using JurayKV.Domain.Aggregates.StateLgaAggregate;
using JurayKV.Persistence.RelationalDB.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.RelationalDB.Repositories
{
     internal sealed class StateRepository : GenericRepository<State>, IStateRepository
    {
        private readonly JurayDbContext _dbContext;

        public StateRepository(JurayDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<State>> GetAllState()
        {
            return await _dbContext.States.ToListAsync();
        }
    }


    internal sealed class LgaRepository : GenericRepository<State>, ILgaRepository
    {
        private readonly JurayDbContext _dbContext;

        public LgaRepository(JurayDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task Create(LocalGoverment entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<LocalGoverment>> GetAllLGA(Guid stateId)
        {
            return await _dbContext.LocalGoverments.Where(x=>x.StatesId == stateId).ToListAsync();
        }

        public Task Update(Guid id, LocalGoverment entity)
        {
            throw new NotImplementedException();
        }

        IQueryable<LocalGoverment> IGenericRepository<LocalGoverment>.GetAll()
        {
            throw new NotImplementedException();
        }

        Task<LocalGoverment> IGenericRepository<LocalGoverment>.GetById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
