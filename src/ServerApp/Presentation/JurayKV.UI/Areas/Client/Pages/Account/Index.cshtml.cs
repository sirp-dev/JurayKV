using JurayKV.Application;
using JurayKV.Application.Queries.UserAccountQueries.DashboardQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.Client.Pages.Account
{
    [Authorize(Policy = Constants.CompanyPolicy)]

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        public IndexModel(ILogger<IndexModel> logger, IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            GetClientDashboardQuery command = new GetClientDashboardQuery(Guid.Parse(userId));

            DashboardData = await _mediator.Send(command);

            return Page();
        }
        public UserDashboardDto DashboardData { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {

            return Page();
        }
    }
}
