using JurayKV.Domain.Aggregates.ImageAggregate;
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
        internal sealed class ImageRepository : GenericRepository<ImageFile>, IImageRepository
    {
        private readonly JurayDbContext _dbContext;

        public ImageRepository(JurayDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> ExistsAsync(Expression<Func<ImageFile, bool>> condition)
        {
            IQueryable<ImageFile> queryable = _dbContext.Set<ImageFile>();

            if (condition != null)
            {
                queryable = queryable.Where(condition);
            }

            return queryable.AnyAsync();
        }

        public async Task<ImageFile> GetByIdAsync(Guid imageId)
        {

            if (imageId == Guid.Empty)
            {
                throw new ArgumentException("imageId cannot be empty.", nameof(imageId));
            }
            //Image image = await _dbContext.Set<Image>().FindAsync(imageId);
            ImageFile image = await _dbContext.ImageFiles.FirstOrDefaultAsync(x => x.Id == imageId);
            return image;
        }

        public async Task InsertAsync(ImageFile image)
        {
            if (image == null)
            {
                throw new ArgumentException("imageId cannot be empty.", nameof(image));
            }

            await _dbContext.AddAsync(image);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(ImageFile image)
        {
            if (image == null)
            {
                throw new ArgumentException("imageId cannot be empty.", nameof(image));
            }

            EntityEntry<ImageFile> trackedEntity = _dbContext.ChangeTracker.Entries<ImageFile>()
                .FirstOrDefault(x => x.Entity == image);

            if (trackedEntity == null)
            {
                _dbContext.Update(image);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(ImageFile image)
        {
            if (image == null)
            {
                throw new ArgumentException("imageId cannot be empty.", nameof(image));
            }

            _dbContext.Remove(image);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ImageFile>> ImageSHowInDropDown()
        {
           return await _dbContext.ImageFiles.Where(x=>x.ShowInDropdown == true).ToListAsync();
        }
    }

}
