using Amazon;
using JurayKV.Application.Queries.IdentityQueries.PermissionQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Commands.IdentityCommands.UserCommands
{
    public sealed class ListPermissionCommand : IRequest<UserPermissionDto>
    {
        public ListPermissionCommand(string userId)
        {
           UserId = userId;
        }

        public string UserId { get; } 

        private class ListPermissionCommandHandler : IRequestHandler<ListPermissionCommand, UserPermissionDto>
        {
            private readonly RoleManager<ApplicationRole> _roleManager;
            private readonly UserManager<ApplicationUser> _userManager;


            public ListPermissionCommandHandler(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
            {
                _roleManager = roleManager;
                _userManager = userManager;
            }

            public async Task<UserPermissionDto> Handle(ListPermissionCommand request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));
                try
                {
                    var user = await _userManager.FindByIdAsync(request.UserId);

                    if (user == null)
                    {
                        // Handle the case where the user with the specified ID is not found.
                        return null;
                    }

                    var allRoles = await _roleManager.Roles.ToListAsync(); // Assuming you are using Entity Framework

                    var userRoles = await _userManager.GetRolesAsync(user);

                    var userPermissionList = new UserPermissionDto
                    {
                        UserId = user.Id.ToString(),
                        Email = user.Email,
                        Fullname = user.SurName +" " + user.FirstName + " " + user.LastName,
                        IdNumber = user.IdNumber,
                        Roles = allRoles.Select(role => new RoleList
                        {
                            Role = role.Name,
                            RoleId = role.Id.ToString(),
                            Selected = userRoles.Contains(role.Name)
                        }).ToList()
                    };
                    return userPermissionList;

                }
                catch (Exception x) { }
                return null;
            }
        }
    }

}
