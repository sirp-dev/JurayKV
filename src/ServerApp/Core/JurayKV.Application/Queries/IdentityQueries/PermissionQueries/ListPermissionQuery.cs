using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Queries.IdentityQueries.PermissionQueries
{


    public sealed class ListPermissionQuery : IRequest<List<PermissionListDto>>
    {


        private class ListPermissionQueryHandler : IRequestHandler<ListPermissionQuery, List<PermissionListDto>>
        {
            private readonly RoleManager<ApplicationRole> _roleManager;


            public ListPermissionQueryHandler(RoleManager<ApplicationRole> roleManager)
            {
                _roleManager = roleManager;
            }

            public async Task<List<PermissionListDto>> Handle(ListPermissionQuery request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));

                var roles = await _roleManager.Roles.Where(x => x.Name != "SuperAdmin").ToListAsync();
                Expression<Func<ApplicationRole, PermissionListDto>> selectExp = d => new PermissionListDto
                {
                    RoleName = d.Name,
                    Id = d.Id,
                };

                List<PermissionListDto> RoleList = roles.Select(selectExp.Compile()).ToList();
                return RoleList;
            }
        }
    }
    public class PermissionListDto
    {
        public string RoleName { get; set; }
        public Guid Id { get; set; }
    }
}

