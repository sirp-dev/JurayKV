using JurayKV.Application;
using JurayKV.Application.Queries.ExchangeRateQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IRates
{
    [Authorize(Policy = Constants.Transaction)]

    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<ExchangeRateListDto> ExchangeRates = new List<ExchangeRateListDto>();
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            GetExchangeRateListQuery command = new GetExchangeRateListQuery();
            ExchangeRates = await _mediator.Send(command);

            return Page();
        }
    }
}
