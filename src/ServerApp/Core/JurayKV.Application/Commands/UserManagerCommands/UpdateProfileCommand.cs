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
    public sealed class UpdateProfileCommand : IRequest<bool>
    {
        public UpdateProfileCommand(Guid id, UpdateProfileDto data)
        {
            Id = id;
            Data = data; 
        }
        public UpdateProfileDto Data { get; set; }
        public Guid Id { get; set; } 
    }

    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserManagerCacheHandler _userManagerCacheHandler;
 
        public UpdateProfileCommandHandler(
UserManager<ApplicationUser> userManager, IUserManagerCacheHandler userManagerCacheHandler)
        {
            _userManager = userManager;
            _userManagerCacheHandler = userManagerCacheHandler;
         }

        public async Task<bool> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
             request.ThrowIfNull(nameof(request));
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
 
            user.SurName = request.Data.Surname;
            user.FirstName = request.Data.Firstname;
            user.LastName = request.Data.Lastname;
            user.Address = request.Data.Address;
            user.State = request.Data.State;
            if(request.Data.LGA != null) { 
            user.LGA = request.Data.LGA;
            }
            user.DateOfBirth = request.Data.DateOfBirth;
            

            var result = await _userManager.UpdateAsync(user);

            //update role
            if (result.Succeeded)
            {
               return true;
            }
            else
            {
              return false;
            }
              
        }
    }
     
}
