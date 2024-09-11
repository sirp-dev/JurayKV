using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.CompanyAggregate;
using JurayKV.Domain.Exceptions;
using JurayKV.Domain.ValueObjects;
using MediatR;
using TanvirArjel.ArgumentChecker;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace JurayKV.Application.Commands.CompanyCommands;

public sealed class UpdateCompanyCommand : IRequest
{
    public UpdateCompanyCommand(
        Guid id,
        string name,
        decimal amountPerPoint
        )
    {
        Id = id;
        Name = name;
        AmountPerPoint = amountPerPoint;
    }

    public Guid Id { get; }

    public string Name { get; }
    public decimal AmountPerPoint { get; set; }

}

internal class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand>
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICompanyCacheHandler _companyCacheHandler;

    public UpdateCompanyCommandHandler(
        ICompanyRepository companyRepository,
        ICompanyCacheHandler companyCacheHandler)
    {
        _companyRepository = companyRepository;
        _companyCacheHandler = companyCacheHandler;
    }

    public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        request.ThrowIfNull(nameof(request));

        Company companyToBeUpdated = await _companyRepository.GetByIdAsync(request.Id);

        if (companyToBeUpdated == null)
        {
            throw new EntityNotFoundException(typeof(Company), request.Id);
        }

        companyToBeUpdated.Name = request.Name;
        companyToBeUpdated.AmountPerPoint = request.AmountPerPoint;
        await _companyRepository.UpdateAsync(companyToBeUpdated);

        // Remove the cache
        await _companyCacheHandler.RemoveListAsync();
        await _companyCacheHandler.RemoveDropdownListAsync();
        await _companyCacheHandler.RemoveGetAsync(companyToBeUpdated.Id);
        await _companyCacheHandler.RemoveDetailsByIdAsync(companyToBeUpdated.Id);
    }
}
