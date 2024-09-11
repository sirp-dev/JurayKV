using JurayKV.Domain.Aggregates.VariationAggregate;
using JurayKV.Domain.Aggregates.VariationAggregate;
using JurayKV.Persistence.RelationalDB.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Persistence.RelationalDB.Repositories
{
    internal sealed class VariationRepository : GenericRepository<Variation>, IVariationRepository
    {
        private readonly JurayDbContext _dbContext;

        public VariationRepository(JurayDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Variation> GetByIdAsync(Guid variationId)
        {

            if (variationId == Guid.Empty)
            {
                throw new ArgumentException("variationId cannot be empty.", nameof(variationId));
            }
             Variation variation = await _dbContext.Variations.Include(x=>x.CategoryVariation).FirstOrDefaultAsync(x => x.Id == variationId);
            return variation;
        }

        public async Task InsertAsync(Variation variation)
        {
            if (variation == null)
            {
                throw new ArgumentException("variationId cannot be empty.", nameof(variation));
            }

            await _dbContext.AddAsync(variation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Variation variation)
        {
            if (variation == null)
            {
                throw new ArgumentException("variationId cannot be empty.", nameof(variation));
            }

            EntityEntry<Variation> trackedEntity = _dbContext.ChangeTracker.Entries<Variation>()
                .FirstOrDefault(x => x.Entity == variation);

            if (trackedEntity == null)
            {
                _dbContext.Update(variation);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Variation variation)
        {
            if (variation == null)
            {
                throw new ArgumentException("variationId cannot be empty.", nameof(variation));
            }

            _dbContext.Remove(variation);
            await _dbContext.SaveChangesAsync();
        }

 

        public async Task<List<Variation>> GetByCategoryByActiveIdAsync(Guid categoryId)
        {
            return await _dbContext.Variations.Where(c => c.CategoryVariationId == categoryId && c.Active == true).ToListAsync();
        }
        public async Task<List<Variation>> GetByCategoryIdAsync(Guid categoryId)
        {
            return await _dbContext.Variations.Where(c => c.CategoryVariationId == categoryId).ToListAsync();
        }

        public async Task<List<Variation>> GetAllListAsync()
        {
            return await _dbContext.Variations.Include(x=>x.CategoryVariation).ToListAsync();
        }
    }
}
