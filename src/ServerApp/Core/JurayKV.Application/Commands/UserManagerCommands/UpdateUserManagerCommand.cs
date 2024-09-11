using JurayKV.Application.Caching.Handlers;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.KvAdAggregate;
using JurayKV.Domain.Aggregates.WalletAggregate;
using JurayKV.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TanvirArjel.ArgumentChecker;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace JurayKV.Application.Commands.UserManagerCommands
{
    public sealed class UpdateUserManagerCommand : IRequest
    {
        public UpdateUserManagerCommand(Guid id, UpdateUserDto data)
        {
            Id = id;
            Data = data;
        }
        public UpdateUserDto Data { get; set; }
        public Guid Id { get; set; }
    }

    internal class UpdateUserManagerCommandHandler : IRequestHandler<UpdateUserManagerCommand>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserManagerCacheHandler _userManagerCacheHandler;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UpdateUserManagerCommandHandler(
UserManager<ApplicationUser> userManager, IUserManagerCacheHandler userManagerCacheHandler, SignInManager<ApplicationUser> signInManager, IWalletRepository walletRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _userManagerCacheHandler = userManagerCacheHandler;
            _signInManager = signInManager;
            _walletRepository = walletRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Handle(UpdateUserManagerCommand request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            user.SurName = request.Data.SurName;
            user.FirstName = request.Data.FirstName;
            user.LastName = request.Data.LastName;
            user.Email = request.Data.Email;
            user.PhoneNumber = request.Data.PhoneNumber;
            user.AccountStatus = request.Data.AccountStatus;
            user.DisableEmailNotification = request.Data.DisableEmailNotification;
            user.Tier = request.Data.Tier;
            user.Tie2Request = request.Data.Tie2Request;

            user.DateUpgraded = request.Data.DateUpgraded;
            user.EmailConfirmed = request.Data.EmailComfirmed;
            user.TwoFactorEnabled = request.Data.TwoFactorEnable;
            user.NinNumber = request.Data.NinNumber;
           


            await _userManager.UpdateAsync(user);
            //

            var loguserId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            string log = user.Email +" "+ user.AccountStatus + " " + user.Tier;
            await _walletRepository.LogUserAsync(log, loguserId, user.Id);

            //remove catch
            await _userManagerCacheHandler.RemoveListAsync();
            await _userManagerCacheHandler.RemoveDetailsByIdAsync(user.Id);
        }
    }
}
