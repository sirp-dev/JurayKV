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
    public class UpdateModel : PageModel
    {

        private readonly IMediator _mediator;
        public UpdateModel(IMediator mediator)
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
                UpdateBucketCommand command = new UpdateBucketCommand(UpdateBucket.Id, UpdateBucket.Name, UpdateBucket.Disable, UpdateBucket.AdminActive, UpdateBucket.UserActive);
                await _mediator.Send(command);
                TempData["success"] = "Updated Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding new bucket";
            }
            return RedirectToPage("./Index");
        }
    }
}
