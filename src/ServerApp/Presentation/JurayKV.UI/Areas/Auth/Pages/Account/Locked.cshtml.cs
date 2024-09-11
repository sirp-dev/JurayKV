using Amazon;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.Auth.Pages.Account
{
    //[Authorize]
    public class LockedModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager; 

        public LockedModel(SignInManager<ApplicationUser> signInManager,

            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
             
        }
         
        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string IdNx { get; set; }
        public async Task<IActionResult> OnGetAsync(string id = null)
        {
            if(id == null)
            {
                return RedirectToPage("/");
            }
            string userId = _userManager.GetUserId(HttpContext.User);
            var getuser = await _userManager.FindByIdAsync(id);
            Name = getuser.SurName + " " + getuser.FirstName;
            IdNx = getuser.Id.ToString();
            // Clear the existing external cookie to ensure a clean login process

            return Page();
        }
    }
}
