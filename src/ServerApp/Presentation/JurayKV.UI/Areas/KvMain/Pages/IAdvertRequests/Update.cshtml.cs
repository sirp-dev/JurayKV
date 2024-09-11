using JurayKV.Application;
using JurayKV.Application.Commands.AdvertRequestCommands;
using JurayKV.Application.Queries.AdvertRequestQueries;
using JurayKV.Application.Queries.CompanyQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JurayKV.UI.Areas.KvMain.Pages.IAdvertRequests
{
    [Authorize(Policy = Constants.AdvertPolicy)]
    public class UpdateModel : PageModel
    {

        private readonly IMediator _mediator;
        public UpdateModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public AdvertRequestDetailsDto Command { get; set; }
        public List<SelectListItem> ListCompanies { get; set; }
        [BindProperty]
        public IFormFile? imagefile { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                GetAdvertRequestByIdQuery command = new GetAdvertRequestByIdQuery(id);
                Command = await _mediator.Send(command);
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
            catch (Exception ex)
            {
                TempData["error"] = "unable to fetch AdvertRequest";
                return RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                UpdateAdvertRequestCommand command = new UpdateAdvertRequestCommand(Command, imagefile);
                await _mediator.Send(command);
                TempData["success"] = "Updated Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding new AdvertRequest";
            }
            return RedirectToPage("./Index");
        }
    }
}
