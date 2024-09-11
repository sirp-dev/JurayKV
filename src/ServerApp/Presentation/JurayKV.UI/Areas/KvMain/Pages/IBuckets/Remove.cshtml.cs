using JurayKV.Application;
using JurayKV.Application.Commands.BucketCommands;
using JurayKV.Application.Queries.BucketQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IBuckets
{
    [Authorize(Policy = Constants.BucketPolicy)]
    public class RemoveModel : PageModel
    {

        private readonly IMediator _mediator;
        public RemoveModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public BucketDetailsDto UpdateBucket { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                GetBucketByIdQuery command = new GetBucketByIdQuery(id);
                UpdateBucket = await _mediator.Send(command);

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
                DeleteBucketCommand command = new DeleteBucketCommand(UpdateBucket.Id);
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
