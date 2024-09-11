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
       public sealed class TieTwoRequestUserManagerCommand : IRequest<ResponseCsaUpgrade>
    {
        public TieTwoRequestUserManagerCommand(Guid id, TieTwoRequestDto data, IFormFile? iDcard, IFormFile? passport)
        {
            Id = id;
            Data = data;
            IDcard = iDcard;
            Passport = passport;
        }
        public TieTwoRequestDto Data { get; set; }
        public Guid Id { get; set; }
        public IFormFile IDcard { get; set; }
        public IFormFile Passport { get; set; }
    }

    internal class TieTwoRequestUserManagerCommandHandler : IRequestHandler<TieTwoRequestUserManagerCommand, ResponseCsaUpgrade>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserManagerCacheHandler _userManagerCacheHandler;
        private readonly IStorageService _storage;

        public TieTwoRequestUserManagerCommandHandler(
UserManager<ApplicationUser> userManager, IUserManagerCacheHandler userManagerCacheHandler, IStorageService storage)
        {
            _userManager = userManager;
            _userManagerCacheHandler = userManagerCacheHandler;
            _storage = storage;
        }

        public async Task<ResponseCsaUpgrade> Handle(TieTwoRequestUserManagerCommand request, CancellationToken cancellationToken)
        {
            ResponseCsaUpgrade response = new ResponseCsaUpgrade();
            request.ThrowIfNull(nameof(request));
            var user = await _userManager.FindByIdAsync(request.Id.ToString());

            try
            {

                var xresult = await _storage.MainUploadFileReturnUrlAsync("", request.IDcard);
                // 
                if (xresult.Message.Contains("200"))
                {
                    user.IDCardUrl = xresult.Url;
                    user.IDCardKey = xresult.Key;
                }
                else
                {
                    response.Message += "<br>Unable to Upload ID Card";
                }

            }
            catch (Exception c)
            {
                response.Message += "<br>Unable to Upload ID Card";
            }
            try
            {

                var xresult = await _storage.MainUploadFileReturnUrlAsync("", request.Passport);
                // 
                if (xresult.Message.Contains("200"))
                {
                    user.PassportUrl = xresult.Url;
                    user.PassportKey = xresult.Key;
                }
                else
                {
                    response.Message += "<br>Unable to Upload passport";
                }

            }
            catch (Exception c)
            {
                response.Message += "<br>Unable to Upload passport";
            }
            user.SurName = request.Data.Surname;
            user.FirstName = request.Data.Firstname;
            user.LastName = request.Data.Lastname;
            user.Address = request.Data.Address;
            user.State = request.Data.State;

            user.LGA = request.Data.LGA;
            user.DateOfBirth = request.Data.DateOfBirth;
            user.Occupation = request.Data.Occupation;
            user.About = request.Data.About;
            user.StateOfOrigin = request.Data.StateOfOrigin;
             
            user.BankName = request.Data.BankName;
            user.AccountNumber = request.Data.AccountNumber;
            user.AccountName = request.Data.AccountName; 
            user.RequestDateTie2Upgraded = DateTime.UtcNow.AddHours(1);
            user.Tie2Request  = Domain.Primitives.Enum.TieRequestStatus.Requested;
            user.ResponseOnTieRequest = "Your Information has been received and is undergoing review within 24hrs.";

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
