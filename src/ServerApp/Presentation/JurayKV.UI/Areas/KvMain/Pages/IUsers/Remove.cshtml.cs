using JurayKV.Application;
using JurayKV.Application.Commands.UserManagerCommands;
using JurayKV.Application.Queries.UserManagerQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IUsers
{
    [Authorize(Policy = Constants.UsersManagerPolicy)]
    public class RemoveModel : PageModel
    {

        private readonly IMediator _mediator;
        public RemoveModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public UserManagerDetailsDto UpdateUserManager { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                GetUserManagerByIdQuery command = new GetUserManagerByIdQuery(id);
                UpdateUserManager = await _mediator.Send(command);

                return Page();
            }
            catch (Exception ex)
            {
                TempData["error"] = "unable to fetch bucket";
                return RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                DeleteUserManagerCommand command = new DeleteUserManagerCommand(UpdateUserManager.Id);
                await _mediator.Send(command);
                TempData["success"] = "Deleted Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. removing new bucket";
            }
            return RedirectToPage("./Index");
        }
    }
}
