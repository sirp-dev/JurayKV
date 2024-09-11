using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Commands.UserMessageCommands;
using JurayKV.Application.Infrastructures;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.UserMessageAggregate;
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
    public sealed class LastLoginCommand : IRequest
    {
        public LastLoginCommand(string userId)
        {
            UserId = userId;

        }
        public string UserId { get; set; }

    }

    internal class LastLoginCommandHandler : IRequestHandler<LastLoginCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserManagerCacheHandler _userManagerCacheHandler;
        private readonly IStorageService _storage;
        private readonly IWalletRepository _walletRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LastLoginCommandHandler(
UserManager<ApplicationUser> userManager, IUserManagerCacheHandler userManagerCacheHandler, IStorageService storage, IWalletRepository walletRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _userManagerCacheHandler = userManagerCacheHandler;
            _storage = storage;
            _walletRepository = walletRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Handle(LastLoginCommand request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));
            try
            {

                var user = await _userManager.FindByIdAsync(request.UserId.ToString());
                user.LastLoggedInAtUtc = DateTime.UtcNow.AddHours(1);
                await _userManager.UpdateAsync(user);

                var loguserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
                string log = user.Email + " " + user.AccountStatus + " " + user.Tier;
                await _walletRepository.LogUserAsync(log, loguserId, user.Id);

            }
            catch { }

        }
    }
}
