using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using JurayKV.Application;
using JurayKV.Application.Queries.IdentityQueries.PermissionQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.Auth.Pages.IRoles
{
    [Authorize(Policy = Constants.SuperAdminPolicy)]

    public class EditRoleModel : PageModel
    {
        private readonly IMediator _mediator;
        public EditRoleModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        [BindProperty]
        public ApplicationRole Role { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            GetPermissionByIdQuery command = new GetPermissionByIdQuery(Guid.Parse(id));
            Role = await _mediator.Send(command);

            if (Role == null)
            {
                return NotFound();
            }
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


            try
            {
                UpdatePermissionQuery command = new UpdatePermissionQuery(Role.Id, Role.Name);
                await _mediator.Send(command);
            }
            catch (Exception c)
            {

                throw;

            }

            return RedirectToPage("./List");
        }

    }
}
