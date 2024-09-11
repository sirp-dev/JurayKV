using JurayKV.Application.Queries.EmployeeQueries;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace JurayKV.Application.Caching.Repositories;

[ScopedService]
public interface IEmployeeCacheRepository
{
    Task<EmployeeDetailsDto> GetDetailsByIdAsync(Guid employeeId);
}
