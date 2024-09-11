using JurayKV.Application;
using JurayKV.Application.Interswitch;
using JurayKvV.Infrastructure.Interswitch.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.Payment.Pages.Account
{
    [Authorize(Policy = Constants.Dashboard)]

    public class BxListAquiryModel : PageModel
    {
        private readonly IMediator _mediator;

        public BxListAquiryModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public BillersByCategoryResponse Billers { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            GetBillersByCategoryQuery getcommand = new GetBillersByCategoryQuery(id);
            Billers = await _mediator.Send(getcommand);
            return Page();
        }
    }
}
