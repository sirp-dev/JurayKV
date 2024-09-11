using JurayKV.Domain.Aggregates.IdentityAggregate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JurayKV.UI.Pages.ViewComponents
{
       public class UserDashboardViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserDashboardViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
           
            var entity =  _userManager.Users.AsQueryable();
            
            ViewBag.all = entity.Count();
            ViewBag.ActiveOnly = entity.Where(x=>x.AccountStatus == Domain.Primitives.Enum.AccountStatus.Active).Count();
             ViewBag.Suspended = entity.Where(x=>x.AccountStatus == Domain.Primitives.Enum.AccountStatus.Suspended).Count();
            ViewBag.Disabled = entity.Where(x=>x.AccountStatus == Domain.Primitives.Enum.AccountStatus.Disabled).Count(); 
            return View();


        }
    }
}
