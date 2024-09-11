using JurayKV.Application;
using JurayKV.Application.Queries.DashboardQueries;
using JurayKV.Application.Queries.UserAccountQueries.DashboardQueries;
using JurayKV.Application.Queries.UserManagerQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.User.Pages.Account
{
    [Authorize(Policy = Constants.Dashboard)]

    public class ReferralHistoryModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReferralHistoryModel(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }
        public List<UserManagerListDto> UserData { get; set; }
        public string Phonenumber { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            var userdata = await _userManager.FindByIdAsync(userId);
            GetUserManagerByReferralListQuery command = new GetUserManagerByReferralListQuery(userdata.PhoneNumber);

            UserData = await _mediator.Send(command);
            Phonenumber = userdata.PhoneNumber;
            return Page();
        }
    }
}
