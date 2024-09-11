using JurayKV.Application;
using JurayKV.Application.Commands.KvAdCommands;
using JurayKV.Application.Queries.BucketQueries;
using JurayKV.Application.Queries.CompanyQueries;
using JurayKV.Application.Queries.ImageQueries;
using JurayKV.Application.Queries.KvAdQueries;
using JurayKV.Domain.Aggregates.KvAdAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.UI.Areas.KvMain.Pages.IAds
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
        public CommandDto Command { get; set; }
        public class CommandDto
        {
            public Guid ImageId { get; set; }
            public Guid UserId { get; set; }
            public Guid BucketId { get; set; }
            public Guid CompanyId { get; set; }
            public DateTime Date { get; set; }
            public DataStatus Status { get; set; }
        } 

        public List<SelectListItem> ListBuckets { get; set; }
        public List<SelectListItem> ListCompanies { get; set; }
        public List<SelectListItem> ListImages { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            GetBucketDropdownListQuery commandbucket = new GetBucketDropdownListQuery();
            List<BucketDropdownListDto> ListBucket = await _mediator.Send(commandbucket);
            // Map CompanyDropdownListDto to SelectListItem
            ListBuckets = ListBucket.Where(x=>x.Disable == false).Select(bucket =>
                new SelectListItem
                {
                    Value = bucket.Id.ToString(), // Assuming Id is an integer
                    Text = bucket.Name
                }).ToList();
            //
            GetCompanyDropdownListQuery commandCompany = new GetCompanyDropdownListQuery();
            List<CompanyDropdownListDto> ListCompany = await _mediator.Send(commandCompany);
            // Map CompanyDropdownListDto to SelectListItem
            ListCompanies = ListCompany.Select(company =>
                new SelectListItem
                {
                    Value = company.Id.ToString(), // Assuming Id is an integer
                    Text = company.Name
                }).ToList();

            //
            //
            GetImageDropDownListQuery commandImages = new GetImageDropDownListQuery();
            List<ImageDto> ImagesList = await _mediator.Send(commandImages);
            // Map CompanyDropdownListDto to SelectListItem
            ListImages = ImagesList.Select(img =>
                new SelectListItem
                {
                    Value = img.Id.ToString(), // Assuming Id is an integer
                    Text = img.Name
                }).ToList();

            //
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier); // Get the user's ID claim

                if (userIdClaim != null)
                {
                    string userId = userIdClaim.Value;
                    CreateKvAdCommand command = new CreateKvAdCommand(Command.ImageId, Guid.Parse(userId), Command.BucketId, Command.CompanyId, Command.Date, Command.Status);
                    Guid Result = await _mediator.Send(command);
                    if(Result == Guid.Empty)
                    {
                        TempData["error"] = "Advert with Date Already Found. Kindly Update the Existing one";
                        return RedirectToPage("./Index");

                    }
                    TempData["success"] = "Added Successfuly";
                    return RedirectToPage("./Index");

                }
                else
                {
                    // The user's ID claim was not found. Handle this case accordingly.
                    TempData["error"] = "user's ID claim was not found";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding new bucket";
            }
            return RedirectToPage("./Index");
        }
    }
}
