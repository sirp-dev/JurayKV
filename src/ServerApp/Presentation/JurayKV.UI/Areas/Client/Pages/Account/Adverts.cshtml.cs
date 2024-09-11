using Azure.Core;
using JurayKV.Application;
using JurayKV.Application.Queries.BucketQueries;
using JurayKV.Application.Queries.CompanyQueries;
using JurayKV.Application.Queries.KvAdQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.Client.Pages.Account
{
    [Authorize(Policy = Constants.CompanyPolicy)]

    public class AdvertsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IMediator _mediator;
        public AdvertsModel(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }
        public BucketDetailsDto UpdateBucket { get; set; }
        public KvAdDetailsDto AdsDto { get; set; }

        public List<KvAdListDto> KvAds = new List<KvAdListDto>();
        public List<KvAdListDto> KvAdsUpcoming = new List<KvAdListDto>();
        public List<KvAdListDto> KvAdsFinished = new List<KvAdListDto>();
        public DateTime CurrentDate { get;set;}
        public async Task<IActionResult> OnGetAsync()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            GetCompanyByUserIdQuery companycomand = new GetCompanyByUserIdQuery(Guid.Parse(userId));
            var company = await _mediator.Send(companycomand);

            GetKvAdListByCompanyIdQuery command = new GetKvAdListByCompanyIdQuery(company.Id);
            KvAds = await _mediator.Send(command);

            DateTime currentDate = DateForSix.GetTheDateBySix(DateTime.UtcNow.AddHours(1));
            CurrentDate = currentDate;
            // Get finished items (where the date is before 6 am today)
            KvAdsFinished = KvAds.Where(x => x.Status == Domain.Primitives.Enum.DataStatus.Active)
                .Where(item => item.CreatedAtUtc.Date < currentDate.Date).OrderByDescending(X => X.CreatedAtUtc)
                .ToList();

            // Get active and upcoming items (where the date is after or exactly 6 am today)
            KvAdsUpcoming = KvAds.Where(x => x.Status == Domain.Primitives.Enum.DataStatus.Active)
                .Where(item => item.CreatedAtUtc.Date >= currentDate.Date).OrderBy(X => X.CreatedAtUtc)
                .ToList();

            KvAds = KvAds.OrderBy(X => X.CreatedAtUtc)
                .ToList();


            return Page();
        }

    }
}
