using JurayKV.Application;
using JurayKV.Application.Interswitch;
using JurayKvV.Infrastructure.Interswitch.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.User.Pages.Account
{
     [Authorize(Policy = Constants.Dashboard)]

    public class BxItemListArxModel : PageModel
    {
        private readonly IMediator _mediator;

        public BxItemListArxModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public BillerPaymentItemResponse BillersItem { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            GetBillerPaymentItemQuery getcommand = new GetBillerPaymentItemQuery(id);
            BillersItem = await _mediator.Send(getcommand);
            return Page();
        }
    }
}
