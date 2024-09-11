using JurayKV.Application.Commands.NotificationCommands;
using JurayKV.Application.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static JurayKV.Domain.Primitives.Enum;
using System.Xml.Linq;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using JurayKV.Application.Infrastructures;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using JurayKV.Application;

namespace JurayKV.UI.Areas.KvMain.Pages.IUsers
{
    [Authorize(Policy = Constants.UsersManagerPolicy)]

    public class ResendAllEmailsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _sender;
        private readonly IMediator _mediator;
        public ResendAllEmailsModel(UserManager<ApplicationUser> userManager, IEmailSender sender, IMediator mediator)
        {
            _userManager = userManager;
            _sender = sender;
            _mediator = mediator;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
             
                try
                {
                var userslist = await _userManager.Users.Where(x=>x.EmailConfirmed == false).ToListAsync();
                foreach (var user in userslist) { 
                    var identityResult = await _userManager.FindByIdAsync("");
                    NotificationType notificationType = (NotificationType)6;
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

                    string maskedEmail = "";
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

                    TempData["codetype"] = codetype;
                    //TempData["success"] = "OTP Sent Successfuly";
                    return RedirectToPage("./Comfirmation", new { xcode = codetype, xmal = maskedEmail, txtd = identityResult.Id });
                }
                }
                catch (Exception exception)
                {

                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            
            // If we got this far, something failed, redisplay form
            return Page();
        }

    }
}
