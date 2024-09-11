using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Infrastructures;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.UserManagerCommands
{
      public sealed class ApproveTieTwoUserManagerCommand : IRequest<ResponseCsaUpgrade>
    {
        public ApproveTieTwoUserManagerCommand(Guid id)
        {
            Id = id; 
        }
         public Guid Id { get; set; }
     
    }

    internal class ApproveTieTwoUserManagerCommandHandler : IRequestHandler<ApproveTieTwoUserManagerCommand, ResponseCsaUpgrade>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserManagerCacheHandler _userManagerCacheHandler;
 
        public ApproveTieTwoUserManagerCommandHandler(
UserManager<ApplicationUser> userManager, IUserManagerCacheHandler userManagerCacheHandler )
        {
            _userManager = userManager;
            _userManagerCacheHandler = userManagerCacheHandler;
         }

        public async Task<ResponseCsaUpgrade> Handle(ApproveTieTwoUserManagerCommand request, CancellationToken cancellationToken)
        {
            ResponseCsaUpgrade response = new ResponseCsaUpgrade();
            request.ThrowIfNull(nameof(request));
            var user = await _userManager.FindByIdAsync(request.Id.ToString());

             
            user.Tier = Domain.Primitives.Enum.Tier.Tier2;
            user.RequestDateTie2Upgraded = DateTime.UtcNow.AddHours(1);

            var result = await _userManager.UpdateAsync(user);

            //update role
            if (result.Succeeded)
            {
                response.Message += "<br>Information Update.";
                response.Success = true;
            }
            else
            {
                response.Message += "<br>Unable to Update";
                response.Success = false;
            }
            //remove catch
            await _userManagerCacheHandler.RemoveListAsync();
            await _userManagerCacheHandler.RemoveDetailsByIdAsync(user.Id);

            return response;
        }
    }
}
