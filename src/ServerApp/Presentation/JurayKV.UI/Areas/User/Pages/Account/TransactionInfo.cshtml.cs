using JurayKV.Application;
using JurayKV.Application.Queries.IdentityKvAdQueries;
using JurayKV.Application.Queries.TransactionQueries;
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


    public class TransactionInfoModel : BasePageModel
    {
        private readonly ILogger<TransactionInfoModel> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        public TransactionInfoModel(ILogger<TransactionInfoModel> logger, IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            GetTransactionByIdQuery command = new GetTransactionByIdQuery(id);
            TransactionDetailsDto = await _mediator.Send(command);



            return Page();
        }
        [BindProperty]
        public TransactionDetailsDto TransactionDetailsDto { get; set; }


    }
}
