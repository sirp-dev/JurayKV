namespace JurayKV.Application.Caching.Handlers
{
    public interface ICompanyCacheHandler
    {
        Task RemoveDetailsByIdAsync(Guid companyId);
        Task RemoveGetAsync(Guid companyId);

        Task RemoveListAsync();
        Task RemoveDropdownListAsync();

    }
}
