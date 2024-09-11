using JurayKV.Application;
using JurayKV.Application.Queries.CompanyQueries;
using JurayKV.Application.Queries.IdentityQueries.PermissionQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.ICompanys
{
    [Authorize(Policy = Constants.CompanyPolicy)]
    public class IndexModel : PageModel
    {

        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<CompanyListDto> Companys = new List<CompanyListDto>();
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            GetCompanyListQuery command = new GetCompanyListQuery();
            Companys = await _mediator.Send(command);

            return Page();
        }
    }
}
