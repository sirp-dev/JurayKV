using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.DepartmentAggregate;
using JurayKV.Domain.Exceptions;
using JurayKV.Domain.ValueObjects;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.DepartmentCommands;

public sealed class UpdateDepartmentCommand : IRequest
{
    public UpdateDepartmentCommand(
        Guid id,
        string name,
        string description,
        bool isActive)
    {
        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
    }

    public Guid Id { get; }

    public string Name { get; }

    public string Description { get; }

    public bool IsActive { get; }
}

internal class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand>
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IDepartmentCacheHandler _departmentCacheHandler;

    public UpdateDepartmentCommandHandler(
        IDepartmentRepository departmentRepository,
        IDepartmentCacheHandler departmentCacheHandler)
    {
        _departmentRepository = departmentRepository;
        _departmentCacheHandler = departmentCacheHandler;
    }

    public async Task Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        request.ThrowIfNull(nameof(request));

        Department departmentToBeUpdated = await _departmentRepository.GetByIdAsync(request.Id);

        if (departmentToBeUpdated == null)
        {
            throw new EntityNotFoundException(typeof(Department), request.Id);
        }

        DepartmentName departmentName = new DepartmentName(request.Name);

        await departmentToBeUpdated.SetNameAsync(_departmentRepository, departmentName);
        departmentToBeUpdated.SetDescription(request.Description);
        departmentToBeUpdated.IsActive = request.IsActive;

        await _departmentRepository.UpdateAsync(departmentToBeUpdated);

        await _departmentCacheHandler.RemoveListAsync();
    }
}
