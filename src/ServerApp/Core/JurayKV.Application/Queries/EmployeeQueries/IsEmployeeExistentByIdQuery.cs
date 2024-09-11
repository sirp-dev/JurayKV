﻿using JurayKV.Domain.Aggregates.EmployeeAggregate;
using MediatR;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.EmployeeQueries;

public sealed class IsEmployeeExistentByIdQuery : IRequest<bool>
{
    public IsEmployeeExistentByIdQuery(Guid employeeId)
    {
        Id = employeeId.ThrowIfEmpty(nameof(employeeId));
    }

    public Guid Id { get; }

    private class IsEmployeeExistentByIdQueryHandler : IRequestHandler<IsEmployeeExistentByIdQuery, bool>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public IsEmployeeExistentByIdQueryHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<bool> Handle(IsEmployeeExistentByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            bool isExistent = await _employeeRepository.ExistsAsync(e => e.Id == request.Id);
            return isExistent;
        }
    }
}
