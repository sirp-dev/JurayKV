using Amazon.Runtime.Internal;
using Azure.Core;
using JurayKV.Application.Caching.Repositories;
using JurayKV.Application.Commands.IdentityCommands.UserCommands;
using JurayKV.Application.Commands.NotificationCommands;
using JurayKV.Application.Commands.TransactionCommands;
using JurayKV.Application.Commands.UserManagerCommands;
using JurayKV.Application.Commands.WalletCommands;
using JurayKV.Application.Queries.IdentityQueries.UserQueries;
using JurayKV.Application.Queries.SettingQueries;
using JurayKV.Application.Queries.TransactionQueries;
using JurayKV.Application.Queries.UserManagerQueries;
using JurayKV.Application.Queries.WalletQueries;
using JurayKV.Application.Services;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.WalletAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.UI.Areas.Auth.Pages.Account
{

    [AllowAnonymous]
    public class ComfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWalletRepository _walletRepository;
        private readonly ISettingCacheRepository _settingCacheRepository;
        public ComfirmationModel(UserManager<ApplicationUser> userManager, IMediator mediator, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor, IWalletRepository walletRepository, ISettingCacheRepository settingCacheRepository)
        {
            _userManager = userManager;
            _mediator = mediator;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _walletRepository = walletRepository;
            _settingCacheRepository = settingCacheRepository;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /// [bind
        /// 
        [BindProperty]
        public Guid Xtxnt { get; set; }
        [BindProperty]
        public string One { get; set; }

        [BindProperty]
        public int NotificationNumber { get; set; }


        [BindProperty]
        public string XC { get; set; }

        [BindProperty]
        public string XM { get; set; }

        [BindProperty]
        public string TX { get; set; }


        public async Task<IActionResult> OnGetAsync(string xcode, string xmal, string txtd)
        {
            if (xmal == null)
            {
                return RedirectToPage("/Index");
            }
            if (txtd == null)
            {
                return RedirectToPage("/Index");

            }
            var data = await _userManager.FindByIdAsync(txtd);

            if (data == null)
            {
                return NotFound($"Unable to load user with email '{xmal}'.");
            }
            XC = xcode;
            XM = xmal;
            TX = txtd;
            Xtxnt = data.Id;
            TempData["codetype"] = xcode;
            return Page();
        }

        [BindProperty]
        public string NewPhoneNumber { get; set; }

        [BindProperty]
        public string OldPhoneNumber { get; set; }
        public async Task<IActionResult> OnPostUpdatePhone(string xcode, string xmal, string txtd)
        {
            var user = await _userManager.FindByIdAsync(txtd);

            if (OldPhoneNumber == user.PhoneNumber)
            {
                string last10DigitsPhoneNumber1 = NewPhoneNumber.Substring(Math.Max(0, NewPhoneNumber.Length - 10));
                var checkphone = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber.Contains(last10DigitsPhoneNumber1));
                if (checkphone == null)
                {
                    user.PhoneNumber = NewPhoneNumber;
                    await _userManager.UpdateAsync(user);
                    TempData["xsuccess"] = "New Phone Number update. Request for whatsapp code.";
                    return RedirectToPage("./Comfirmation", new { xcode = xcode, xmal = xmal, txtd = user.Id });

                }
                else
                {
                    TempData["xerror"] = "New Phone Number is Already Used";
                    return RedirectToPage("./Comfirmation", new { xcode = xcode, xmal = xmal, txtd = user.Id });
                }
            }
            else
            {
                TempData["xerror"] = "Old Phone Number is wrong";
                return RedirectToPage("./Comfirmation", new { xcode = xcode, xmal = xmal, txtd = user.Id });

            }

        }
        public async Task<IActionResult> OnPostSendEmailAsync()
        {
             
                try
                {
                    var identityResult = await _userManager.FindByIdAsync(Xtxnt.ToString());
                    NotificationType notificationType = (NotificationType)NotificationNumber;
                    CreateNotificationCommand command = new CreateNotificationCommand(identityResult.Id, notificationType);

                    DataResponseDto result = await _mediator.Send(command);
                    //


                    if (result.BoolResponse == true)
                    {
                        TempData["success"] = "Code Sent Successfuly.";

                    }
                    else
                    {
                        TempData["error"] = "Unable to Send Email. Click Resend Again";
                    }

                    string maskedEmail = "dxvl-a";
                    string codetype = "";
                    if (notificationType == NotificationType.Email)
                    {
                        codetype = "Email";
                    }
                    else if (notificationType == NotificationType.SMS)
                    {
                        codetype = "SMS";
                    }
                    else if (notificationType == NotificationType.Voice)
                    {
                        codetype = "Voice";
                    }
                    else if (notificationType == NotificationType.Whatsapp)
                    {
                        codetype = "Whatsapp";
                    }
                    TempData["codetype"] = codetype;
                    //TempData["success"] = "OTP Sent Successfuly";
                    return RedirectToPage("./Comfirmation", new { xcode = codetype, xmal = maskedEmail, txtd = identityResult.Id });
                }
                catch (Exception exception)
                {

                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                var identityResult = await _userManager.FindByIdAsync(Xtxnt.ToString());
                if (identityResult.EmailConfirmed == true)
                {

                    return RedirectToPage("./Login");
                }
                GetEmailVerificationCodeQuery command = new GetEmailVerificationCodeQuery(identityResult.Id.ToString());

                EmailVerificationCode result = await _mediator.Send(command);
                //
                string VerificationCode = One;

                VerificationCode = VerificationCode.Replace(" ", "");
                result.Code = result.Code.Replace(" ", "");
                if (result.Code == VerificationCode)
                {
                    identityResult.EmailConfirmed = true;
                    identityResult.AccountStatus = AccountStatus.New;
                    identityResult.Role = "SMA";
                    await _userManager.UpdateAsync(identityResult);


                    //if (DateTime.UtcNow > result.SentAtUtc.AddMinutes(5))
                    //{
                    //    TempData["error"] = "The code is expired.";
                    //    return Page();
                    //}
                    //UpdateEmailVerificationCodeCommand verificationcommand = new UpdateEmailVerificationCodeCommand(result.Email, result.PhoneNumber, result.Id.ToString(), result.Code, result.Id, DateTime.UtcNow);
                    //bool verificationresult = await _mediator.Send(verificationcommand);
                    await _signInManager.SignInAsync(identityResult, true);
                    LastLoginCommand lst = new LastLoginCommand(identityResult.Id.ToString());
                    await _mediator.Send(lst);
                    return RedirectToPage("/Account/Index", new { area = "User" });
                }
                else
                {
                    TempData["error"] = "Invalid Code";
                    return Page();
                }

            }
            catch (Exception exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }



        }
    }
}
