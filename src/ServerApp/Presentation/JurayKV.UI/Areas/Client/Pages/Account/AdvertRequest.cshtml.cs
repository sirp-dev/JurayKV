using JurayKV.Application;
using JurayKV.Application.Commands.AdvertRequestCommands;
using JurayKV.Application.Queries.CompanyQueries;
using JurayKV.Application.Queries.SettingQueries;
using JurayKV.Domain.Aggregates.AdvertRequestAggregate;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.Client.Pages.Account
{
      [Authorize(Policy = Constants.CompanyPolicy)]
    public class AdvertRequestModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        public AdvertRequestModel(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }



        [BindProperty]
        public AdvertRequest Command { get; set; }
         [BindProperty]
        public IFormFile? imagefile { get; set; }

        public SettingDetailsDto SettingDetails { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            GetSettingDefaultQuery settingcommand = new GetSettingDefaultQuery();
            SettingDetails = await _mediator.Send(settingcommand);

            return Page();
        }
        [BindProperty]
        public BankInfo BankInfoResult { get; set; }

        public class BankInfo
        {
            public string? BankAccount { get; set; }
            public string? BankName { get; set; }
            public string? BankAccountNumber { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            GetSettingDefaultQuery settingcommand = new GetSettingDefaultQuery();
            SettingDetails = await _mediator.Send(settingcommand);

            if(Command.Amount < SettingDetails.MinimumAmountBudget)
            {
                TempData["error"] = "Amount Lower Than Budget";
                return Page();
            }
            try
            {
                string userId = _userManager.GetUserId(HttpContext.User);
                CreateCompanyAdvertRequestCommand command = new CreateCompanyAdvertRequestCommand(Command, imagefile, Guid.Parse(userId));
                AdvertResponse Result = await _mediator.Send(command);

                if(Result.PaymentGateWay == Domain.Primitives.Enum.PaymentGateway.Flutterwave)
                {
                    return Redirect(Result.FlutterResponseDto.data.link);
                }
                else if (Result.PaymentGateWay == Domain.Primitives.Enum.PaymentGateway.Bank)
                {
                    
                    return RedirectToPage("./PaymentInfo");

                }
                
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding new AdvertRequest";
            }
            return RedirectToPage("./Index");
        }
    }
}
