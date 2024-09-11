using JurayKV.Application.Queries.DepartmentQueries;
using JurayKV.Domain.Aggregates.DepartmentAggregate;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace JurayKV.Application.Caching.Repositories;

[ScopedService]
public interface IDepartmentCacheRepository
{
    Task<List<DepartmentDto>> GetListAsync();

    Task<Department> GetByIdAsync(Guid departmentId);

    Task<DepartmentDetailsDto> GetDetailsByIdAsync(Guid departmentId);
}
