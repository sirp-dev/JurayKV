﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.DepartmentAggregate;

public interface IDepartmentRepository
{
    Task<Department> GetByIdAsync(Guid departmentId);

    Task<bool> ExistsAsync(Expression<Func<Department, bool>> condition);

    Task InsertAsync(Department department);

    Task UpdateAsync(Department department);

    Task DeleteAsync(Department department);
}
