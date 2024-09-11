using JurayKV.Domain.Aggregates.CompanyAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JurayKV.Domain.Aggregates.CompanyAggregate
{
    public interface ICompanyRepository
    {
        Task<Company> GetByIdAsync(Guid companyId);
        Task<Company> GetByUserIdAsync(Guid userId);
        Task InsertAsync(Company company);

        Task UpdateAsync(Company company);

        Task DeleteAsync(Company company);
    }
}
