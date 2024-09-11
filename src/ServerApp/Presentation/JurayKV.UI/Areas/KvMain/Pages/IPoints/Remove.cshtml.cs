using JurayKV.Application;
using JurayKV.Application.Commands.KvPointCommands;
using JurayKV.Application.Queries.KvPointQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IPoints
{
    [Authorize(Policy = Constants.PointPolicy)]
    public class RemoveModel : PageModel
    {

        private readonly IMediator _mediator;
        public RemoveModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public KvPointDetailsDto UpdateKvPoint { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                GetKvPointByIdQuery command = new GetKvPointByIdQuery(id);
                UpdateKvPoint = await _mediator.Send(command);

                return Page();
            }
            catch (Exception ex)
            {
                TempData["error"] = "unable to fetch bucket";
                return RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                DeleteKvPointCommand command = new DeleteKvPointCommand(UpdateKvPoint.Id);
                await _mediator.Send(command);
                TempData["success"] = "Deleted Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. removing new bucket";
            }
            return RedirectToPage("./Index");
        }
    }
}
