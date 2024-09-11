using JurayKV.Application;
using JurayKV.Application.Queries.UserManagerQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IUsers
{
    [Authorize(Policy = Constants.SuperAdminPolicy)]
    public class UpdateRolesModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _user;

        public UpdateRolesModel(UserManager<ApplicationUser> user)
        {
            _user = user;
        }

        [BindProperty]
        public UserManagerDetailsDto UpdateUserManager { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
               var users = _user.Users.ToList();
                foreach (var user in users)
                {

                    user.Role = "SMA";
                    await _user.UpdateAsync(user);
                }

                return Page();
            }
            catch (Exception ex)
            {
                TempData["error"] = "unable to fetch bucket";
                return RedirectToPage("/Index");
            }
        }
    }
}
