using JurayKV.Application;
using JurayKV.Application.Queries.SliderQueries;
using JurayKV.Application.Queries.UserMessageQueries;
using JurayKV.Domain.Aggregates.UserMessageAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IUserMessages
{
    [Authorize(Policy = Constants.AdminPolicy)]
    public class InfoModel : PageModel
    {

        private readonly IMediator _mediator;
        public InfoModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public UserMessage UserMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                GetMessageByIdQuery command = new GetMessageByIdQuery(id);
                UserMessage = await _mediator.Send(command);

                return Page();
            }
            catch (Exception ex)
            {
                TempData["error"] = "unable to fetch data";
                return RedirectToPage("/Index");
            }
        }

    }
}
