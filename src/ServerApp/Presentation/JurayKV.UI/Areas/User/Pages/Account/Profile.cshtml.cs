using JurayKV.Application;
using JurayKV.Application.Queries.DashboardQueries;
using JurayKV.Application.Queries.UserAccountQueries.DashboardQueries;
using JurayKV.Application.Queries.UserManagerQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.UI.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.User.Pages.Account
{
      [Authorize(Policy = Constants.Dashboard)]
 public class ProfileModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProfileModel(ILogger<IndexModel> logger, IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            GetUserManagerByIdQuery command = new GetUserManagerByIdQuery(Guid.Parse(userId));

            UserData = await _mediator.Send(command); 
            GetUserDashboardQuery dcommand = new GetUserDashboardQuery(Guid.Parse(userId));

            DashboardData = await _mediator.Send(dcommand);
            return Page();
        }
        public UserManagerDetailsDto UserData { get; set; }
        public UserDashboardDto DashboardData { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {

            return Page();
        }
    }
}
