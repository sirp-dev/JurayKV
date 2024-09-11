using JurayKV.Application.Commands.NotificationCommands;
using JurayKV.Application.Infrastructures;
using JurayKV.Application.Services;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.UI.Areas.Auth.Pages.Account
{
      [AllowAnonymous]
    public class VerifyPhoneModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWhatsappOtp _sender;
        private readonly IMediator _mediator;
        public VerifyPhoneModel(UserManager<ApplicationUser> userManager, IWhatsappOtp sender, IMediator mediator)
        {
            _userManager = userManager;
            _sender = sender;
            _mediator = mediator;
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
        public int NotificationNumber { get; set; }
        public string RefX { get; set; }


        public async Task<IActionResult> OnGetAsync(string xmal, string txtd, string refx = null)
        {
            RefX = refx;
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

            Xtxnt = data.Id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
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

                    string maskedEmail = MaskEmail(identityResult.Email);
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
                    else if (notificationType == NotificationType.Voice)
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
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
        public static string MaskEmail(string email)
        {
            int atIndex = email.IndexOf('@');
            if (atIndex < 0)
            {
                // Invalid email address, return it as is
                return email;
            }

            string prefix = email.Substring(0, atIndex);
            string domain = email.Substring(atIndex);

            // Replace characters in the middle with "xxxx"
            int lengthToMask = Math.Max(prefix.Length - 2, 0); // Leave at least 1 character before the '@'
            string maskedPrefix = new string('x', lengthToMask) + email[atIndex - 1];

            return maskedPrefix + domain;
        }

    }
}
