using JurayKV.Application;
using JurayKV.Application.Commands.IdentityKvAdCommands;
using JurayKV.Application.Queries.BucketQueries;
using JurayKV.Application.Queries.KvAdQueries;
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


    public class RegisterAdsModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        public RegisterAdsModel(ILogger<IndexModel> logger, IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            CreateIdentityKvAdCommand command = new CreateIdentityKvAdCommand(null, Guid.Parse(userId), id, DateTime.UtcNow.AddHours(1));

            var outcome = await _mediator.Send(command);
            return new JsonResult("Successfuly");
        }
       

    }

      
}
