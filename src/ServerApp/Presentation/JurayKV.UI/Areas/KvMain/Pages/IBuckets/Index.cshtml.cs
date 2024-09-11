using JurayKV.Application;
using JurayKV.Application.Commands.BucketCommands;
using JurayKV.Application.Queries.BucketQueries;
using JurayKV.Application.Queries.IdentityQueries.PermissionQueries;
using JurayKV.Application.Queries.KvAdQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IBuckets
{
    [Authorize(Policy = Constants.BucketPolicy)]
    public class IndexModel : PageModel
    {

        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<BucketListDto> Buckets = new List<BucketListDto>();
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            GetBucketListQuery command = new GetBucketListQuery();
            Buckets = await _mediator.Send(command);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                ClearActiveAdsQuery xcommand = new ClearActiveAdsQuery();
              await _mediator.Send(xcommand);
                TempData["success"] = "Added Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding new bucket";
            }
            return RedirectToPage("./Index");
        }
    }
}
