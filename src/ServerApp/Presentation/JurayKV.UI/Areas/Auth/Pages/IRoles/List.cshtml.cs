using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JurayKV.Application;
using JurayKV.Application.Queries.DashboardQueries;
using JurayKV.Application.Queries.IdentityQueries.PermissionQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace JurayKV.UI.Areas.Auth.Pages.IRoles
{
    [Authorize(Policy = Constants.SuperAdminPolicy)]
    public class ListModel : PageModel
    {

        private readonly IMediator _mediator;
        public ListModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<PermissionListDto> Roles = new List<PermissionListDto>();
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            ListPermissionQuery command = new ListPermissionQuery();
            Roles = await _mediator.Send(command);

            return Page();
        }
    }
}
