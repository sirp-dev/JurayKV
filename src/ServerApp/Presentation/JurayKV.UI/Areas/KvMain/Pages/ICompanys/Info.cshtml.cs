using JurayKV.Application;
using JurayKV.Application.Commands.CompanyCommands;
using JurayKV.Application.Queries.CompanyQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.ICompanys
{
    [Authorize(Policy = Constants.CompanyPolicy)]
    public class InfoModel : PageModel
    {

        private readonly IMediator _mediator;
        public InfoModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public CompanyDetailsDto UpdateCompany { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                GetCompanyByIdQuery command = new GetCompanyByIdQuery(id);
                UpdateCompany = await _mediator.Send(command);

                return Page();
            }
            catch (Exception ex)
            {
                TempData["error"] = "unable to fetch bucket";
                return RedirectToPage("/Index");
            }
        }

     }
}
