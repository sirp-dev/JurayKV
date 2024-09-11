using JurayKV.Application.Queries.IdentityQueries.PermissionQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IPermissions
{
    //[Authorize(Roles = "mSuperAdmin")]

    public class CreateRoleModel : PageModel
    {
        private readonly IMediator _mediator;
        public CreateRoleModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        [BindProperty]
        public ApplicationRole Role { get; set; }
        public IActionResult OnGet()
        {
            return Page();
        }

       

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            CreatePermissionQuery command = new CreatePermissionQuery(Role.Name);
            await _mediator.Send(command);

            return RedirectToPage("./Index");
        }
    }
}