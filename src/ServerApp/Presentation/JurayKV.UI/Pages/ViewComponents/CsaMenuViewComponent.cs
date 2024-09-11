using JurayKV.Application.Queries.KvAdQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JurayKV.UI.Pages.ViewComponents
{
     public class CsaMenuViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public CsaMenuViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string LoggedInUser = _userManager.GetUserId(HttpContext.User);
            bool user = false;
            var entity = await _userManager.FindByIdAsync(LoggedInUser);
            if (entity != null) {
                user = await _userManager.IsInRoleAsync(entity, "CSA");
                }

            ViewBag.CSAuser = user;
                return View();


        }
    }
}
