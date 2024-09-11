using JurayKV.Domain.Aggregates.BucketAggregate;
using JurayKV.Domain.Aggregates.CompanyAggregate;
using JurayKV.Persistence.RelationalDB.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Persistence.RelationalDB.Repositories
{
    internal sealed class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        private readonly JurayDbContext _dbContext;

        public CompanyRepository(JurayDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

 

            public Task<bool> ExistsAsync(Expression<Func<Company, bool>> condition)
        {
            IQueryable<Company> queryable = _dbContext.Set<Company>();

            if (condition != null)
            {
                queryable = queryable.Where(condition);
            }

            return queryable.AnyAsync();
        }

        public async Task<Company> GetByIdAsync(Guid companyId)
        {
            companyId.ThrowIfEmpty(nameof(companyId));

            Company company = await _dbContext.Set<Company>().FindAsync(companyId);
            return company;
        }

        public async Task<Company> GetByUserIdAsync(Guid userId)
        {
            userId.ThrowIfEmpty(nameof(userId));

            Company company = await _dbContext.Companies.Include(c=>c.User).FirstOrDefaultAsync(x=>x.UserId == userId);
            return company;
        }

        public async Task InsertAsync(Company company)
        {
            company.ThrowIfNull(nameof(company));

            await _dbContext.AddAsync(company);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Company company)
        {
            company.ThrowIfNull(nameof(company));

            EntityEntry<Company> trackedEntity = _dbContext.ChangeTracker.Entries<Company>()
                .FirstOrDefault(x => x.Entity == company);

            if (trackedEntity == null)
            {
                _dbContext.Update(company);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Company company)
        {
            company.ThrowIfNull(nameof(company));

            _dbContext.Remove(company);
            await _dbContext.SaveChangesAsync();
        }
    }

}
