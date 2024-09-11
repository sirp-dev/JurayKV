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
    public sealed class SeedPermissionQuery : IRequest
    {
        public SeedPermissionQuery()
        {
        }

    }

    internal class SeedPermissionQueryHandler : IRequestHandler<SeedPermissionQuery>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public SeedPermissionQueryHandler(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task Handle(SeedPermissionQuery request, CancellationToken cancellationToken)
        {
            _ = request.ThrowIfNull(nameof(request));
            try
            {
                ApplicationRole role1 = new ApplicationRole
                {
                    Name = "SuperAdmin"
                };

                var result1 = await _roleManager.CreateAsync(role1);

                if (result1.Succeeded)
                {

                }
            }
            catch (Exception s)
            {

            }
            try
            {
                ApplicationRole role1 = new ApplicationRole
                {
                    Name = "Manager"
                };

                var result1 = await _roleManager.CreateAsync(role1);

                if (result1.Succeeded)
                {

                }
            }
            catch (Exception s)
            {

            }
            try
            {
                ApplicationRole role1 = new ApplicationRole
                {
                    Name = "Admin"
                };

                var result1 = await _roleManager.CreateAsync(role1);

                if (result1.Succeeded)
                {

                }
            }
            catch (Exception s)
            {

            }
            try
            {
                ApplicationRole role1 = new ApplicationRole
                {
                    Name = "Company"
                };

                var result1 = await _roleManager.CreateAsync(role1);

                if (result1.Succeeded)
                {

                }
            }
            catch (Exception s)
            {

            }
            try
            {
                ApplicationRole role1 = new ApplicationRole
                {
                    Name = "Bucket"
                };

                var result1 = await _roleManager.CreateAsync(role1);

                if (result1.Succeeded)
                {

                }
            }
            catch (Exception s)
            {

            }
            try
            {
                ApplicationRole role1 = new ApplicationRole
                {
                    Name = "ExchangeRate"
                };

                var result1 = await _roleManager.CreateAsync(role1);

                if (result1.Succeeded)
                {

                }
            }
            catch (Exception s)
            {

            }
            try
            {
                ApplicationRole role1 = new ApplicationRole
                {
                    Name = "Advert"
                };

                var result1 = await _roleManager.CreateAsync(role1);

                if (result1.Succeeded)
                {

                }
            }
            catch (Exception s)
            {

            }
            try
            {
                ApplicationRole role1 = new ApplicationRole
                {
                    Name = "UsersManager"
                };

                var result1 = await _roleManager.CreateAsync(role1);

                if (result1.Succeeded)
                {

                }
            }
            catch (Exception s)
            {

            }
            try
            {
                ApplicationRole role1 = new ApplicationRole
                {
                    Name = "Client"
                };

                var result1 = await _roleManager.CreateAsync(role1);

                if (result1.Succeeded)
                {

                }
            }
            catch (Exception s)
            {

            }
            try
            {
                ApplicationRole role1 = new ApplicationRole
                {
                    Name = "User"
                };

                var result1 = await _roleManager.CreateAsync(role1);

                if (result1.Succeeded)
                {

                }
            }
            catch (Exception s)
            {

            }
        }
    }
}
