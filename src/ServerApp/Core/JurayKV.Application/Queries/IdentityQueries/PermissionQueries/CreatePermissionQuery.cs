using JurayKV.Domain.Aggregates.IdentityAggregate;
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

    public sealed class CreatePermissionQuery : IRequest<Guid>
    {
        public CreatePermissionQuery(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    internal class CreatePermissionQueryHandler : IRequestHandler<CreatePermissionQuery, Guid>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public CreatePermissionQueryHandler(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Guid> Handle(CreatePermissionQuery request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));
            ApplicationRole role = new ApplicationRole
            {
                Name = request.Name
            };

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return role.Id;
            }
            else
            {
                return Guid.Empty;
            }


        }
    }
}


