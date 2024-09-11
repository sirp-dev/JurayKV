using JurayKV.Application;
using JurayKV.Application.Commands.IdentityKvAdCommands;
using JurayKV.Application.Queries.BucketQueries;
using JurayKV.Application.Queries.IdentityKvAdQueries;
using JurayKV.Application.Queries.KvAdQueries;
using JurayKV.Application.Queries.UserAccountQueries.DashboardQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.UI.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;

namespace JurayKV.UI.Areas.User.Pages.Account
{
    [Authorize(Policy = Constants.Dashboard)]


    public class AdsModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdsModel(ILogger<IndexModel> logger, IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync(Guid kvId)
        {
            // Retrieve the value from TempData
            if (TempData.ContainsKey("TempKvId"))
            {
                TempData.TryGetValue("TempKvId", out var tempKvId);
                if (Guid.TryParse(tempKvId?.ToString(), out var parsedTempKvId))
                {
                    // Clear the TempData value
                    TempData.Remove("TempKvId");

                    // Use the parsedTempKvId in your logic
                    // ...
                    TempData["success"] = "Ads Registered Successfuly";
                    return RedirectToPage("./Bucket");
                }
            }

            // Your existing logic when kvId is present
            if (kvId != Guid.Empty)
            {
                GetActiveAdsQuery command = new GetActiveAdsQuery(kvId);

                Ads = await _mediator.Send(command);

                //// Include CheckExistByUserIdAndAdsQuery to check if the user exists in the list
                //bool userExists = await _mediator.Send(new CheckExistByUserIdAndAdsQuery
                //{
                //    UserId = userId,
                //    KvAdsId = kvId
                //});

                //// Set the Exist property in each KvAdListDto
                //Ads.ForEach(ad => ad.Exist = userExists);



                GetBucketByIdQuery cmd = new GetBucketByIdQuery(kvId);
                Bucket = await _mediator.Send(cmd);
                return Page();
            }
            else
            {
                return RedirectToPage("./Bucket");
            }
        }
        public KvAdDetailsDto Ads { get; set; }
        public BucketDetailsDto Bucket { get; set; }
        [BindProperty]
        public Guid kId { get; set; }
        [BindProperty]
        public DateTime Date {  get; set; }
        //public Guid kId { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            CreateIdentityKvAdCommand command = new CreateIdentityKvAdCommand(null, Guid.Parse(userId), kId, Date);

            var outcome = await _mediator.Send(command);
            if (outcome != Guid.Empty)
            {


                // Store the kId in TempData
                TempData["TempKvId"] = kId.ToString();

                GetKvAdByIdQuery getad = new GetKvAdByIdQuery(kId);
                var data = await _mediator.Send(getad);
                return Redirect(data.ImageUrl);

            }
            else
            {
                TempData["error"] = "Unable to Register Ad or Ad Already Posted";
                return RedirectToPage("./Bucket");
            }
        }

    }

}
