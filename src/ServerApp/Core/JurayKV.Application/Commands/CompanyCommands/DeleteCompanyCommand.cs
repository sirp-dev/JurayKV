using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.CompanyAggregate;
using JurayKV.Domain.Exceptions;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.CompanyCommands;

public sealed class DeleteCompanyCommand : IRequest
{
    public DeleteCompanyCommand(Guid companyId)
    {
        Id = companyId.ThrowIfEmpty(nameof(companyId));
    }

    public Guid Id { get; }
}

internal class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICompanyCacheHandler _companyCacheHandler;

    public DeleteCompanyCommandHandler(ICompanyRepository companyRepository, ICompanyCacheHandler companyCacheHandler)
    {
        _companyRepository = companyRepository;
        _companyCacheHandler = companyCacheHandler;
    }

    public async Task Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        _ = request.ThrowIfNull(nameof(request));

        Company company = await _companyRepository.GetByIdAsync(request.Id);

        if (company == null)
        {
            throw new EntityNotFoundException(typeof(Company), request.Id);
        }

        await _companyRepository.DeleteAsync(company); 
        
        // Remove the cache
        await _companyCacheHandler.RemoveListAsync();
        await _companyCacheHandler.RemoveDropdownListAsync();
        await _companyCacheHandler.RemoveGetAsync(company.Id); 
        await _companyCacheHandler.RemoveDetailsByIdAsync(company.Id);
    }
}