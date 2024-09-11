using JurayKV.Application;
using JurayKV.Application.Queries.DashboardQueries;
using JurayKV.UI.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.Dashboard
{
    [Authorize(Policy = Constants.Dashboard)]

    public class IndexModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;
        public IndexModel(ILogger<IndexModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            DashboardQuery command = new DashboardQuery();

            DashboardDto data = await _mediator.Send(command);
            DashboardData = data;
            return Page();
        }
        public DashboardDto DashboardData { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {

            return Page();
        }
    }
}
