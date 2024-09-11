using JurayKV.Application;
using JurayKV.Application.Commands.IdentityKvAdCommands;
using JurayKV.Application.Queries.BucketQueries;
using JurayKV.Application.Queries.IdentityKvAdQueries;
using JurayKV.Application.Queries.KvAdQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.ValueObjects;
using JurayKV.UI.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JurayKV.UI.Areas.User.Pages.Account
{
    [Authorize(Policy = Constants.Dashboard)]


    public class BucketModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        public BucketModel(ILogger<IndexModel> logger, IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
        }
        [BindProperty]
        public Guid kId { get; set; }
        [BindProperty]
        public DateTime Date { get; set; }
        public async Task<IActionResult> OnGetAsync(string? error, string? success)
        {
 
            string userId = _userManager.GetUserId(HttpContext.User);
            

             DateTime currentDate = DateForSix.GetTheDateBySix(DateTime.UtcNow.AddHours(1));
            GetKvAdActiveListAllBucketQuery command = new GetKvAdActiveListAllBucketQuery(currentDate);

           KvAds = await _mediator.Send(command);

            GetIdentityKvAdActiveByUserIdListQuery kvcommand = new GetIdentityKvAdActiveByUserIdListQuery(Guid.Parse(userId));

           var Ads = await _mediator.Send(kvcommand);

            var myactiveads = Ads.Select(a => a.KvAdId).ToList();
            KvAds.ForEach(ad =>
            {
                ad.MyActiveAdvert = myactiveads.Contains(ad.Id);
            });
             
            TempData["error"] = error;
            TempData["success"] = success;
            return Page();
        }
        public List<KvAdDetailsDto> KvAdDetailsDto { get; set; }
        public List<KvAdListDto> KvAds = new List<KvAdListDto>();

        public async Task<IActionResult> OnPostAsync()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            CreateIdentityKvAdCommand command = new CreateIdentityKvAdCommand(null, Guid.Parse(userId), kId, Date);

            var outcome = await _mediator.Send(command);
            if (outcome != Guid.Empty)
            {


                // Store the kId in TempData
                TempData["TempKvId"] = kId.ToString();
                TempData.Keep();
                GetKvAdByIdQuery getad = new GetKvAdByIdQuery(kId);
                var data = await _mediator.Send(getad);
                return Redirect(data.ImageUrl);

            }
            else
            {
                TempData["error"] = "Unable to Register Ad or Ad Already Posted";
                TempData.Keep();
                return RedirectToPage("./Bucket");
            }
        }

    }

}
