using JurayKV.Application;
using JurayKV.Application.Commands.UserManagerCommands;
using JurayKV.Application.Queries.UserManagerQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayKV.UI.Areas.KvMain.Pages.IUsers
{
    [Authorize(Policy = Constants.UsersManagerPolicy)]
    public class UpdateModel : PageModel
    {

        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateModel(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [BindProperty]
        public UserManagerDetailsDto UpdateUserManager { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                GetUserManagerByIdQuery command = new GetUserManagerByIdQuery(id);
                UpdateUserManager = await _mediator.Send(command);

                return Page();
            }
            catch (Exception ex)
            {
                TempData["error"] = "unable to fetch bucket";
                return RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!String.IsNullOrEmpty(UpdateUserManager.NinNumber))
            {
                var checknin = await _userManager.Users.FirstOrDefaultAsync(x => x.NinNumber == UpdateUserManager.NinNumber);
                if (checknin != null)
                {
                    TempData["error"] = "error. NIN already exist";

                    return RedirectToPage("./Update", new { id = UpdateUserManager.Id });
                }
            }
            try
            {
                UpdateUserDto update = new UpdateUserDto();
                update.AccountStatus = UpdateUserManager.AccountStatus;
                update.Email = UpdateUserManager.Email;
                update.PhoneNumber = UpdateUserManager.PhoneNumber;
                update.SurName = UpdateUserManager.Surname;
                update.FirstName = UpdateUserManager.Firstname;
                update.LastName = UpdateUserManager.Lastname;
                update.DisableEmailNotification = UpdateUserManager.DisableEmailNotification;
                update.Tier = UpdateUserManager.Tier;
                update.DateUpgraded = DateTime.UtcNow.AddHours(1);
                update.Tie2Request = UpdateUserManager.Tie2Request;
                update.EmailComfirmed = UpdateUserManager.EmailComfirmed;
                update.TwoFactorEnable = UpdateUserManager.TwoFactorEnable;
                update.NinNumber = UpdateUserManager.NinNumber;
                UpdateUserManagerCommand command = new UpdateUserManagerCommand(UpdateUserManager.Id, update);
                await _mediator.Send(command);
                TempData["success"] = "Updated Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding updating";
            }
            return RedirectToPage("./Info", new { id = UpdateUserManager.Id });
        }
    }
}
