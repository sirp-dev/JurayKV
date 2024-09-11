using JurayKV.Application;
using JurayKV.Application.Queries.AdvertRequestQueries;
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

    public class AdvertRequestHistoryModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IMediator _mediator;
        public AdvertRequestHistoryModel(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }
        
        public List<AdvertRequestListDto> AdvertRequestListDto = new List<AdvertRequestListDto>();
        
        public async Task<IActionResult> OnGetAsync()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            GetCompanyByUserIdQuery companycomand = new GetCompanyByUserIdQuery(Guid.Parse(userId));
            var company = await _mediator.Send(companycomand);

            GetAdvertRequestListByCompanyIdQuery command = new GetAdvertRequestListByCompanyIdQuery(company.Id);
            AdvertRequestListDto = await _mediator.Send(command);

            

            return Page();
        }

    }
}
