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
    public sealed class GetPermissionByIdQuery : IRequest<ApplicationRole>
    {
        public GetPermissionByIdQuery(
            Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
         
    }

    internal class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, ApplicationRole>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public GetPermissionByIdQueryHandler(
RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ApplicationRole> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));

            var role = await _roleManager.FindByIdAsync(request.Id.ToString());
            
            return role;
        }
    }

}
