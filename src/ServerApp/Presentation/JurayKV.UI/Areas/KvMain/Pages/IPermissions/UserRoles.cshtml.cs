using JurayKV.Application;
using JurayKV.Application.Commands.ExchangeRateCommands;
using JurayKV.Application.Commands.IdentityCommands.UserCommands;
using JurayKV.Application.Queries.IdentityQueries.PermissionQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IPermissions
{
    [Authorize(Policy = Constants.Permission)]
    public class UserRolesModel : PageModel
    {

        private readonly IMediator _mediator;
        public UserRolesModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public UserPermissionDto UserPermissionDto { get; set; }

        [BindProperty]
        public string RoleId { get; set; }
        [BindProperty]
        public string UserId { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            ListPermissionCommand command = new ListPermissionCommand(id);
            UserPermissionDto = await _mediator.Send(command);

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {

            try
            {

                ChangePermissionCommand command = new ChangePermissionCommand(UserId, RoleId);
                bool Result = await _mediator.Send(command);
                if(Result) { 
                TempData["success"] = "Successfuly";
                }
                else
                {
                    TempData["error"] = "error. unable to process";
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = "error. unable to process";
            }
            return RedirectToPage("./UserRoles", new {id = UserId});
        }
    }
}
