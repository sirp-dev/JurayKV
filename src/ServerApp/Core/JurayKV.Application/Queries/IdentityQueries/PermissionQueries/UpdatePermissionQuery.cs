using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.DepartmentAggregate;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Exceptions;
using JurayKV.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.IdentityQueries.PermissionQueries
{


    public sealed class UpdatePermissionQuery : IRequest
    {
        public UpdatePermissionQuery(
            Guid id,
            string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; }

        public string Name { get; }
    }

    internal class UpdatePermissionQueryHandler : IRequestHandler<UpdatePermissionQuery>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UpdatePermissionQueryHandler(
RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task Handle(UpdatePermissionQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            var role = await _roleManager.FindByIdAsync(request.Id.ToString());
            role.Name = request.Name;
            await _roleManager.UpdateAsync(role);
        }
    }

}
