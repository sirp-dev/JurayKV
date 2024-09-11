using JurayKV.Application.Queries.IdentityQueries.PermissionQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.IdentityCommands.UserCommands
{
    public sealed class ChangePermissionCommand : IRequest<bool>
    {
        public ChangePermissionCommand(string userId, string roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public string UserId { get; }
        public string RoleId { get; }

        private class ChangePermissionCommandHandler : IRequestHandler<ChangePermissionCommand, bool>
        {
            private readonly RoleManager<ApplicationRole> _roleManager;
            private readonly UserManager<ApplicationUser> _userManager;


            public ChangePermissionCommandHandler(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
            {
                _roleManager = roleManager;
                _userManager = userManager;
            }

            public async Task<bool> Handle(ChangePermissionCommand request, CancellationToken cancellationToken)
            {
                request.ThrowIfNull(nameof(request));
                try
                {
                    var user = await _userManager.FindByIdAsync(request.UserId);
                    var role = await _roleManager.FindByIdAsync(request.RoleId);
                    if (user == null)
                    {
                        // Handle the case where the user with the specified ID is not found.
                        return false;
                    }

                    var checkuserroles = await _userManager.IsInRoleAsync(user, role.Name);
                    if (checkuserroles == true)
                    {
                        try
                        {
                          var xc =  await _userManager.RemoveFromRoleAsync(user, role.Name);
                            if(xc.Succeeded) { 
                                
                                var getcurUserRoles = await _userManager.GetRolesAsync(user);
                                var rolescount = getcurUserRoles.Where(x=> !x.Contains("CSA") || !x.Contains("SMA") || !x.Contains("Client")).Count();
                                if(rolescount > 0)
                                {
                                    user.Role = "Administrator";
                                }
                                else
                                {
                                    user.Role = "SMA";
                                }
                                
                                return true;}
                        }
                        catch (Exception d) { }
                    }
                    else
                    {
                        try
                        {
                           var dx = await _userManager.AddToRoleAsync(user, role.Name);
                            if (dx.Succeeded) {

                                var getcurUserRoles = await _userManager.GetRolesAsync(user);
                                var rolescount = getcurUserRoles.Where(x => !x.Contains("CSA") || !x.Contains("SMA") || !x.Contains("Client")).Count();
                                if (rolescount > 0)
                                {
                                    user.Role = "Administrator";
                                }
                                else
                                {
                                    user.Role = "SMA";
                                }
                                return true; }
                        }
                        catch (Exception d) { }
                    }
                }
                catch (Exception x) { }
                return false;
            }
        }
    }

}
