using JurayKV.Application;
using JurayKV.Application.Commands.AdvertRequestCommands;
using JurayKV.Application.Queries.AdvertRequestQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IAdvertRequests
{
    [Authorize(Policy = Constants.AdvertPolicy)]
    public class RemoveModel : PageModel
    {

        private readonly IMediator _mediator;
        public RemoveModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public AdvertRequestDetailsDto UpdateAdvertRequest { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                GetAdvertRequestByIdQuery command = new GetAdvertRequestByIdQuery(id);
                UpdateAdvertRequest = await _mediator.Send(command);

                return Page();
            }
            catch (Exception ex)
            {
                TempData["error"] = "unable to fetch AdvertRequest";
                return RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                DeleteAdvertRequestCommand command = new DeleteAdvertRequestCommand(UpdateAdvertRequest.Id);
                await _mediator.Send(command);
                TempData["success"] = "Deleted Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. removing new AdvertRequest";
            }
            return RedirectToPage("./Index");
        }
    }
}
