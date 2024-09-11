using JurayKV.Application;
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
    public class PaymentInfoModel : PageModel
    {
         private readonly IMediator _mediator;

        public PaymentInfoModel(IMediator mediator)
        {
            _mediator = mediator;
         }

         
        public SettingDetailsDto SettingDetails { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            GetSettingDefaultQuery settingcommand = new GetSettingDefaultQuery();
            SettingDetails = await _mediator.Send(settingcommand);

            return Page();
        }
    }
}
