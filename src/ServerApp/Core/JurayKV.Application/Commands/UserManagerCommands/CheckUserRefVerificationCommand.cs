using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Commands.UserMessageCommands;
using JurayKV.Application.Infrastructures;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.UserMessageAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;

namespace JurayKV.Application.Commands.UserManagerCommands
{
     public sealed class CheckUserRefVerificationCommand : IRequest<bool>
    {
        public CheckUserRefVerificationCommand(string referralPhone)
        {
            ReferralPhone = referralPhone;
           
        }
        public string ReferralPhone { get; set; }
        
    }

    internal class CheckUserRefVerificationCommandHandler : IRequestHandler<CheckUserRefVerificationCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserManagerCacheHandler _userManagerCacheHandler;
        private readonly IStorageService _storage;

        public CheckUserRefVerificationCommandHandler(
UserManager<ApplicationUser> userManager, IUserManagerCacheHandler userManagerCacheHandler, IStorageService storage)
        {
            _userManager = userManager;
            _userManagerCacheHandler = userManagerCacheHandler;
            _storage = storage;
        }

        public async Task<bool> Handle(CheckUserRefVerificationCommand request, CancellationToken cancellationToken)
        { 
            request.ThrowIfNull(nameof(request));
             try
            {
                if(request.ReferralPhone != null) { 
                string last10DigitsPhoneNumber1 = request.ReferralPhone.Substring(Math.Max(0, request.ReferralPhone.Length - 10));

                var userref = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber.Contains(last10DigitsPhoneNumber1));
                if (userref != null)
                {
                     var check = await _userManager.Users.Where(x=>x.RefferedByPhoneNumber.Contains(last10DigitsPhoneNumber1) && x.EmailConfirmed == false).CountAsync();
                        if(check > 3) {
                            UserMessage cmdDto = new UserMessage();
                            cmdDto.UserId = userref.Id;
                            cmdDto.Title = "New Referral User Error";
                            cmdDto.Message = "Your new referral cant use your referral link because you have some pending downlines that have not verified their emails";
                            cmdDto.Date = DateTime.UtcNow.AddHours(1);
                            CreateUserMessageCommand newusermsgCommand = new CreateUserMessageCommand(cmdDto, null);

                            return true;

                        }
                }
                }
            }
            catch { }

            return false;
        }
    }
}
