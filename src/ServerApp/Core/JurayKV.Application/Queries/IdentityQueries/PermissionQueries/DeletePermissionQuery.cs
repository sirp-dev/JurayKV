using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.BucketAggregate;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Exceptions;
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
 

public sealed class DeletePermissionQuery : IRequest
    {
        public DeletePermissionQuery(Guid roleId)
        {
            RoleId = roleId.ThrowIfEmpty(nameof(roleId));
        }

        public Guid RoleId { get; }
    }

    internal class DeletePermissionQueryHandler : IRequestHandler<DeletePermissionQuery>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public DeletePermissionQueryHandler(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task Handle(DeletePermissionQuery request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));
            var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
            await _roleManager.DeleteAsync(role);
        }
    }
}
