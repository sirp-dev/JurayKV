using JurayKV.Application;
using JurayKV.Application.Commands.AdvertRequestCommands;
using JurayKV.Application.Queries.AdvertRequestQueries;
using JurayKV.Application.Queries.CompanyQueries;
using JurayKV.Domain.Aggregates.AdvertRequestAggregate;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.WalletAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.UI.Areas.KvMain.Pages.IAdvertRequests
{

    [Authorize(Policy = Constants.AdvertPolicy)]
    public class AddModel : PageModel
    {

        private readonly IMediator _mediator;
        public AddModel(IMediator mediator)
        {
            _mediator = mediator;
        }

         

        [BindProperty]
        public AdvertRequest Command { get; set; }
        public List<SelectListItem> ListCompanies { get; set; }
        [BindProperty]
        public IFormFile? imagefile { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            GetCompanyDropdownListQuery commandCompany = new GetCompanyDropdownListQuery();
            List<CompanyDropdownListDto> ListCompany = await _mediator.Send(commandCompany);
            // Map CompanyDropdownListDto to SelectListItem
            ListCompanies = ListCompany.Select(company =>
                new SelectListItem
                {
                    Value = company.Id.ToString(), // Assuming Id is an integer
                    Text = company.Name
                }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            try
            {
                CreateAdvertRequestCommand command = new CreateAdvertRequestCommand(Command, imagefile);
                Guid Result = await _mediator.Send(command);
                TempData["success"] = "Added Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding new AdvertRequest";
            }
            return RedirectToPage("./Index");
        }
    }
}
