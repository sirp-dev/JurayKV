using JurayKV.Domain.Aggregates.CategoryVariationAggregate;
using JurayKV.Persistence.RelationalDB.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using JurayKV.Domain.Aggregates.CategoryVariationAggregate;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.Persistence.RelationalDB.Repositories
{
       internal sealed class CategoryVariationRepository : GenericRepository<CategoryVariation>, ICategoryVariationRepository
    {
        private readonly JurayDbContext _dbContext;

        public CategoryVariationRepository(JurayDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

 
        public async Task<CategoryVariation> GetByIdAsync(Guid categoryVariationId)
        {

            if (categoryVariationId == Guid.Empty)
            {
                throw new ArgumentException("categoryVariationId cannot be empty.", nameof(categoryVariationId));
            } 
            CategoryVariation categoryVariation = await _dbContext.CategoryVariations.FirstOrDefaultAsync(x => x.Id == categoryVariationId);
            return categoryVariation;
        }

        public async Task InsertAsync(CategoryVariation categoryVariation)
        {
            if (categoryVariation == null)
            {
                throw new ArgumentException("categoryVariationId cannot be empty.", nameof(categoryVariation));
            }

            await _dbContext.AddAsync(categoryVariation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(CategoryVariation categoryVariation)
        {
            if (categoryVariation == null)
            {
                throw new ArgumentException("categoryVariationId cannot be empty.", nameof(categoryVariation));
            }

            EntityEntry<CategoryVariation> trackedEntity = _dbContext.ChangeTracker.Entries<CategoryVariation>()
                .FirstOrDefault(x => x.Entity == categoryVariation);

            if (trackedEntity == null)
            {
                _dbContext.Update(categoryVariation);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(CategoryVariation categoryVariation)
        {
            if (categoryVariation == null)
            {
                throw new ArgumentException("categoryVariationId cannot be empty.", nameof(categoryVariation));
            }

            _dbContext.Remove(categoryVariation);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<CategoryVariation>> GetByTypeByActiveAsync(Domain.Primitives.Enum.VariationType variationType)
        {
            return await _dbContext.CategoryVariations.Where(c => c.VariationType == variationType && c.Active == true).ToListAsync();
        }

        public async Task<List<CategoryVariation>> GetByTypeAsync(Domain.Primitives.Enum.VariationType variationType)
        {
            return await _dbContext.CategoryVariations.Where(c=>c.VariationType == variationType).ToListAsync();
        }

        public async Task<List<CategoryVariation>> GetAllListAsync()
        {
            return await _dbContext.CategoryVariations.ToListAsync();
        }
        public async Task<List<CategoryVariation>> GetAllListByBillerAsync(BillGateway biller)
        {
            return await _dbContext.CategoryVariations.Where(x=>x.BillGateway == biller).ToListAsync();
        }
    }

}
