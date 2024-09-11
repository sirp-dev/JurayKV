using JurayKV.Application;
using JurayKV.Application.Commands.IdentityKvAdCommands;
using JurayKV.Application.Queries.IdentityKvAdQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.UI.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.User.Pages.Account
{
    [Authorize(Policy = Constants.Dashboard)]


    public class UploadVideoProofModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        public UploadVideoProofModel(ILogger<IndexModel> logger, IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
        }
        public IFormFile VideoFile { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            GetIdentityKvAdByIdQuery command = new GetIdentityKvAdByIdQuery(id);
            IdentityKvAdDetailsDto = await _mediator.Send(command);


            DateTime currentDate = DateTime.UtcNow.AddHours(1);
            DateTime nextDay6AM = currentDate.Date.AddDays(1).AddHours(6);

            bool isLinkEnabled = IdentityKvAdDetailsDto.CreatedAtUtc < nextDay6AM;
            if (!isLinkEnabled)
            {
                TempData["error"] = "Time has elapsed";
                return RedirectToPage("./RunningAds");

            }

            return Page();
        }
        [BindProperty]
        public IdentityKvAdDetailsDto IdentityKvAdDetailsDto { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {


            string userId = _userManager.GetUserId(HttpContext.User);

            UpdateIdentityKvAdCommand command = new UpdateIdentityKvAdCommand(IdentityKvAdDetailsDto.Id, VideoFile);
            await _mediator.Send(command);
            return RedirectToPage("./UploadVideoProof", new { id = IdentityKvAdDetailsDto.Id });
        }


    }

}
