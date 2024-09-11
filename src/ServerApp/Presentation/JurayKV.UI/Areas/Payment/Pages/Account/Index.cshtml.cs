using JurayKV.Application;
using JurayKV.Application.Interswitch;
using JurayKV.Application.Queries.SettingQueries;
using JurayKV.Application.Queries.UserManagerQueries;
using JurayKV.Application.VtuServices;
using JurayKV.Domain.Aggregates.CategoryVariationAggregate;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKvV.Infrastructure.Interswitch.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.Payment.Pages.Account
{
    [Authorize(Policy = Constants.Dashboard)]

    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        public BillerCategoryListResponse Billers { get; set; }
        public SettingDetailsDto SettingDetails { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        public UserManagerDetailsDto UserManagerDetailsDto { get;set; }
        public List<CategoryVariation> CategoryVariations { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            GetUserManagerByIdQuery command = new GetUserManagerByIdQuery(Guid.Parse(userId));

            UserManagerDetailsDto = await _mediator.Send(command);
            //if(UserData.AccountStatus == Domain.Primitives.Enum.AccountStatus.Suspended)
            //{
            //    return RedirectToPage("Account/Locked", new { id = UserData.Id, area="Auth" });
            //}

            GetSettingDefaultQuery settingcommand = new GetSettingDefaultQuery();
            SettingDetails = await _mediator.Send(settingcommand);

            if (SettingDetails == null)
            {
                TempData["error"] = "Uable to Validate";
                return RedirectToPage("/Account/Index", new {area="User" });
            }

            if (SettingDetails.BillGateway == Domain.Primitives.Enum.BillGateway.VTU)
            {
                GetVariationCategoryCommand categorycommand = new GetVariationCategoryCommand(Domain.Primitives.Enum.BillGateway.VTU);
                CategoryVariations = await _mediator.Send(categorycommand);
            }
            else
            {
                ListBillersCategoryQuery getcommand = new ListBillersCategoryQuery();
                Billers = await _mediator.Send(getcommand);
            }
            return Page();
        }
    }
}
