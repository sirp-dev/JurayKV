using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.IdentityKvAdAggregate;
using JurayKV.Domain.Aggregates.WalletAggregate;
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
        public sealed class DeleteUserManagerCommand : IRequest
    {
        public DeleteUserManagerCommand(Guid id)
        {
            Id = id; 
        } 
        public Guid Id { get; set; }
    }

    internal class DeleteUserManagerCommandHandler : IRequestHandler<DeleteUserManagerCommand>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityKvAdRepository _identityKvAdRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserManagerCacheHandler _userManagerCacheHandler;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public DeleteUserManagerCommandHandler(
UserManager<ApplicationUser> userManager, IUserManagerCacheHandler userManagerCacheHandler, SignInManager<ApplicationUser> signInManager, IWalletRepository walletRepository, IHttpContextAccessor httpContextAccessor, IIdentityKvAdRepository identityKvAdRepository)
        {
            _userManager = userManager;
            _userManagerCacheHandler = userManagerCacheHandler;
            _signInManager = signInManager;
            _walletRepository = walletRepository;
            _httpContextAccessor = httpContextAccessor;
            _identityKvAdRepository = identityKvAdRepository;
        }

        public async Task Handle(DeleteUserManagerCommand request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));
            var user = await _userManager.FindByIdAsync(request.Id.ToString());

            
            //
             await _identityKvAdRepository.DeleteUserAsync(user.Id);

            var roles = await _userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }


            await _userManager.DeleteAsync(user);
            //remove catch
            await _userManagerCacheHandler.RemoveListAsync();
            await _userManagerCacheHandler.RemoveDetailsByIdAsync(user.Id);
        }
    }
}
