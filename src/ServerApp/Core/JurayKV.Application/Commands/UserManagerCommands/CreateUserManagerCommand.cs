using JurayKV.Application.Caching.Handlers;
using JurayKV.Application.Commands.IdentityCommands.UserCommands;
using JurayKV.Application.Commands.WalletCommands;
using JurayKV.Application.Infrastructures;
using JurayKV.Application.Queries.IdentityQueries.UserQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.WalletAggregate;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.EFCore.GenericRepository;

namespace JurayKV.Application.Commands.UserManagerCommands
{
    public sealed class CreateUserManagerCommand : IRequest<ResponseCreateUserDto>
    {
        public CreateUserManagerCommand(CreateUserDto data)
        {
            Data = data;
        }
        public CreateUserDto Data { get; set; }
    }

    internal class CreateUserManagerCommandHandler : IRequestHandler<CreateUserManagerCommand, ResponseCreateUserDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserManagerCacheHandler _userManagerCacheHandler;
        private readonly IRepository _repository;
        private readonly IWalletRepository _wallet;
        private readonly IMediator _mediator;
        private readonly IEmailSender _emailSender;

        public CreateUserManagerCommandHandler(
UserManager<ApplicationUser> userManager, IUserManagerCacheHandler userManagerCacheHandler, IRepository repository, IMediator mediator, IWalletRepository wallet, IEmailSender emailSender)
        {
            _userManager = userManager;
            _userManagerCacheHandler = userManagerCacheHandler;
            _repository = repository;
            _mediator = mediator;
            _wallet = wallet;
            _emailSender = emailSender;
        }

        public async Task<ResponseCreateUserDto> Handle(CreateUserManagerCommand request, CancellationToken cancellationToken)
        {
            request.ThrowIfNull(nameof(request));
            IDbContextTransaction dbContextTransaction = await _repository
               .BeginTransactionAsync(IsolationLevel.Unspecified, cancellationToken);
            ResponseCreateUserDto response = new ResponseCreateUserDto();
            try
            {


                // Create a random number generator
                Random random = new Random();

                // Generate a 6-digit verification code
                int xverificationCode = random.Next(100000, 1000000);
                Guid newGuid = Guid.NewGuid();

                var checkphone = await _wallet.CheckPhoneUnique(request.Data.PhoneNumber);
                if (checkphone == true)
                {
                    response.Succeeded = false;
                    response.Message = " Phone Number Already Used...";

                }
                var checkemail = await _wallet.CheckEmailUnique(request.Data.Email);
                if (checkemail == true)
                {
                    response.Succeeded = false;
                    response.Message = response.Message + " Email Already Used...";

                }

                if (checkemail == true || checkphone == true)
                {
                    return response;
                }


                int randomNumber = RandomNumberGenerator.GetInt32(0, 1000000);
                string vCode = randomNumber.ToString("D6", CultureInfo.InvariantCulture);
                string verificationCode = string.Join("", vCode.ToCharArray());
                verificationCode = verificationCode.TrimEnd();
                string vcode = $"Your Koboview OTP is {verificationCode}";
                string numbercode = verificationCode;



                ApplicationUser applicationUser = new ApplicationUser
                {
                    SurName = request.Data.Surname,
                    FirstName = request.Data.Firstname,
                    UserName = request.Data.Email,
                    PhoneNumber = request.Data.PhoneNumber,
                    Email = request.Data.Email,
                    Xvalue = verificationCode.ToString(),
                    XvalueDate = DateTime.UtcNow.AddHours(1).AddMinutes(20),
                    XtxtGuid = newGuid.ToString(),
                    CreationUTC = DateTime.UtcNow.AddHours(1),
                    EmailConfirmed = request.Data.Comfirm,
                    RefferedByPhoneNumber = request.Data.RefPhone,
                    Tier = Domain.Primitives.Enum.Tier.Tier1,
                    State = request.Data.State,
                    LGA = request.Data.LGA,
                    Address = request.Data.Address,
                    AccountStatus = Domain.Primitives.Enum.AccountStatus.New
                };

                IdentityResult identityResult = await _userManager.CreateAsync(applicationUser, request.Data.Password);

                if (identityResult.Succeeded == true)
                {
                    var lastUser = await _userManager.Users
                                  .OrderByDescending(u => u.CreationUTC)
                                  .FirstOrDefaultAsync(x => x.IdNumber != null);

                    if (lastUser != null)
                    {
                        // Increment the last user's IdNumber by 1
                        int newIdNumber = int.Parse(lastUser.IdNumber) + 1;

                        // Update the IdNumber for the current user
                        var currentUser = await _userManager.FindByIdAsync(applicationUser.Id.ToString());
                        currentUser.IdNumber = newIdNumber.ToString("00000000");
                        await _userManager.UpdateAsync(currentUser);

                    }


                    await _userManager.AddToRoleAsync(applicationUser, request.Data.Role);
                    response.Id = applicationUser.Id;
                    response.Succeeded = true;
                }
                else
                {
                    var errorBuilder = new StringBuilder();
                    response.Succeeded = false;
                    foreach (var error in identityResult.Errors)
                    {
                        response.Message = error.Description;
                    }

                    return response;
                }


                var user = await _userManager.FindByEmailAsync(applicationUser.Email);
                //create wallet
                CreateWalletCommand walletcommand = new CreateWalletCommand(user.Id, "", "wallet created on " + DateTime.UtcNow, 0);
                Guid Result = await _mediator.Send(walletcommand);
                //

                if (request.Data.Role == "Company")
                {
                   
                }
                else
                {

                    GetEmailVerificationCodeQuery command = new GetEmailVerificationCodeQuery(user.Id.ToString());

                    EmailVerificationCode getexistingVcode = await _mediator.Send(command);
                    if (getexistingVcode == null)
                    {

                        SendEmailVerificationCodeCommand verificationcommand = new SendEmailVerificationCodeCommand(user.Email, user.PhoneNumber, user.Id.ToString(), verificationCode);
                        bool verificationresult = await _mediator.Send(verificationcommand);
                        user.VerificationCode = vcode;
                    }
                    else
                    {
                        user.VerificationCode = $"Your Koboview OTP is {getexistingVcode.Code}";
                    }


                    await _userManager.UpdateAsync(user);
                    //send email.
                    bool result = await _emailSender.SendAsync(vcode, user.Id.ToString(), "Email Comfirmation");

                }

                string maskedEmail = EmailMask.MaskEmail(applicationUser.Email);

                response.Id = applicationUser.Id;
                response.Mxemail = maskedEmail;
                //remove catch
                await _userManagerCacheHandler.RemoveListAsync();
                await _userManagerCacheHandler.RemoveDetailsByIdAsync(applicationUser.Id);
                await dbContextTransaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await dbContextTransaction.RollbackAsync(cancellationToken);
                throw;
            }
            return response;
        }
    }

    public class ResponseCreateUserDto
    {
        public bool Succeeded { get; set; }
        public string Mxemail { get; set; }
        public string Message { get; set; }
        public Guid Id { get; set; }
    }

}
