using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JurayKV.Domain.Aggregates.DepartmentAggregate;
using JurayKV.Domain.Aggregates.EmployeeAggregate;
using JurayKV.Persistence.RelationalDB.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Persistence.RelationalDB.Repositories;

internal sealed class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
{
    private readonly JurayDbContext _dbContext;

    public EmployeeRepository(JurayDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Employee> GetByIdAsync(Guid employeeId)
    {
        employeeId.ThrowIfEmpty(nameof(employeeId));

        Employee employee = await _dbContext.Set<Employee>().FindAsync(employeeId);
        return employee;
    }

    public async Task<bool> ExistsAsync(Expression<Func<Employee, bool>> condition)
    {
        IQueryable<Employee> queryable = _dbContext.Set<Employee>();

        if (condition != null)
        {
            queryable = queryable.Where(condition);
        }

        return await queryable.AnyAsync();
    }

    public async Task InsertAsync(Employee employee)
    {
        employee.ThrowIfNull(nameof(employee));

        await _dbContext.Set<Employee>().AddAsync(employee);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Employee employeeToBeUpdated)
    {
        employeeToBeUpdated.ThrowIfNull(nameof(employeeToBeUpdated));

        EntityEntry<Employee> trackedEntity = _dbContext.ChangeTracker.Entries<Employee>()
           .FirstOrDefault(x => x.Entity == employeeToBeUpdated);

        if (trackedEntity == null)
        {
            _dbContext.Update(employeeToBeUpdated);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Employee employeeToBeDeleted)
    {
        employeeToBeDeleted.ThrowIfNull(nameof(employeeToBeDeleted));

        _dbContext.Set<Employee>().Remove(employeeToBeDeleted);
        await _dbContext.SaveChangesAsync();
    }
}
