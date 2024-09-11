using JurayKV.Application;
using JurayKV.Application.Queries.BucketQueries;
using JurayKV.Application.Queries.KvAdQueries;
using JurayKV.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IBuckets
{
    [Authorize(Policy = Constants.BucketPolicy)]
    public class AdsInBucketModel : PageModel
    {

        private readonly IMediator _mediator;
        public AdsInBucketModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        public BucketDetailsDto UpdateBucket { get; set; }
        public KvAdDetailsDto AdsDto { get; set; }

        public List<KvAdListDto> KvAds = new List<KvAdListDto>();
        public List<KvAdListDto> KvAdsUpcoming = new List<KvAdListDto>();
        public List<KvAdListDto> KvAdsFinished = new List<KvAdListDto>();
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            GetKvAdListByBucketIdQuery command = new GetKvAdListByBucketIdQuery(id);
            KvAds = await _mediator.Send(command);

            DateTime currentDate = DateForSix.GetTheDateBySix(DateTime.UtcNow.AddHours(1));
             
            // Get finished items (where the date is before 6 am today)
            KvAdsFinished = KvAds.Where(x=>x.Status == Domain.Primitives.Enum.DataStatus.Active)
                .Where(item => item.CreatedAtUtc.Date < currentDate.Date).OrderByDescending(X => X.CreatedAtUtc)
                .ToList();

            // Get active and upcoming items (where the date is after or exactly 6 am today)
            KvAdsUpcoming = KvAds.Where(x => x.Status == Domain.Primitives.Enum.DataStatus.Active)
                .Where(item => item.CreatedAtUtc.Date >= currentDate.Date).OrderBy(X=>X.CreatedAtUtc)
                .ToList();


            GetBucketByIdQuery xcommand = new GetBucketByIdQuery(id);
            UpdateBucket = await _mediator.Send(xcommand);

            GetActiveAdsQuery ycommand = new GetActiveAdsQuery(id);
            AdsDto = await _mediator.Send(ycommand);
            return Page();
        }
        [BindProperty]
        public Guid BucketId { get; set; }

        [BindProperty]
        public Guid AdsId { get; set; }

        [BindProperty]
        public bool Active { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                MakeActiveQuery xcommand = new MakeActiveQuery(AdsId, BucketId, Active);
                await _mediator.Send(xcommand);
                TempData["success"] = "Added Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding new bucket";
            }
            return RedirectToPage("./AdsInBucket", new {id= BucketId});
        }
    }
}
