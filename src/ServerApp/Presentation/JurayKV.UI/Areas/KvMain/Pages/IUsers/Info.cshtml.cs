using JurayKV.Application;
using JurayKV.Application.Queries.CompanyQueries;
using JurayKV.Application.Queries.UserManagerQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IUsers
{

    //[Authorize(Policy = Constants.CompanyPolicy)]
    //[Authorize(Policy = Constants.UsersManagerPolicy)]
    [Authorize(Roles = "SuperAdmin,Company,UsersManager,Transaction")]
    public class InfoModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IMediator _mediator;
        public InfoModel(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        [BindProperty]
        public FullUserManagerDetailsDto UpdateUserManager { get; set; }
        public CompanyDetailsDto UpdateCompany { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                GetFullUserManagerByIdQuery command = new GetFullUserManagerByIdQuery(id);
                UpdateUserManager = await _mediator.Send(command);

                 
                return Page();
            }
            catch (Exception ex)
            {
                TempData["error"] = "unable to fetch bucket";
                return RedirectToPage("./Index");
            }
        }

     }
}
