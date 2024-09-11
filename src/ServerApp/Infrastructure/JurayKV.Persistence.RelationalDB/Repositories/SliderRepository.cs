using JurayKV.Domain.Aggregates.SliderAggregate;
using JurayKV.Persistence.RelationalDB.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Persistence.RelationalDB.Repositories
{
    internal sealed class SliderRepository : GenericRepository<Slider>, ISliderRepository
    {
        private readonly JurayDbContext _dbContext;

        public SliderRepository(JurayDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> ExistsAsync(Expression<Func<Slider, bool>> condition)
        {
            IQueryable<Slider> queryable = _dbContext.Set<Slider>();

            if (condition != null)
            {
                queryable = queryable.Where(condition);
            }

            return queryable.AnyAsync();
        }

        public async Task<Slider> GetByIdAsync(Guid sliderId)
        {

            if (sliderId == Guid.Empty)
            {
                throw new ArgumentException("sliderId cannot be empty.", nameof(sliderId));
            }
            //Slider slider = await _dbContext.Set<Slider>().FindAsync(sliderId);
            Slider slider = await _dbContext.Sliders.FirstOrDefaultAsync(x => x.Id == sliderId);
            return slider;
        }

        public async Task InsertAsync(Slider slider)
        {
            if (slider == null)
            {
                throw new ArgumentException("sliderId cannot be empty.", nameof(slider));
            }

            await _dbContext.AddAsync(slider);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Slider slider)
        {
            if (slider == null)
            {
                throw new ArgumentException("sliderId cannot be empty.", nameof(slider));
            }

            EntityEntry<Slider> trackedEntity = _dbContext.ChangeTracker.Entries<Slider>()
                .FirstOrDefault(x => x.Entity == slider);

            if (trackedEntity == null)
            {
                _dbContext.Update(slider);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Slider slider)
        {
            if (slider == null)
            {
                throw new ArgumentException("sliderId cannot be empty.", nameof(slider));
            }

            _dbContext.Remove(slider);
            await _dbContext.SaveChangesAsync();
        }
    }
 
}
