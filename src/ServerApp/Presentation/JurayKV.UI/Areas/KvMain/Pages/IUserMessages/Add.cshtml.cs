using JurayKV.Application;
using JurayKV.Application.Commands.UserMessageCommands;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.SliderAggregate;
using JurayKV.Domain.Aggregates.UserMessageAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IUserMessages
{
    [Authorize(Policy = Constants.AdminPolicy)]
    public class AddModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IMediator _mediator;
        public AddModel(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }
        [BindProperty]
        public string UserId { get; set; }
        public ApplicationUser UserData { get; set; }
        [BindProperty]
        public UserMessage UserMessage { get; set; }

        [BindProperty]
        public IFormFile? datafile { get; set; } 

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user != null)
            {
                UserId = user.Id.ToString();
                UserData = user;
            }
           
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                CreateUserMessageCommand command = new CreateUserMessageCommand(UserMessage, datafile);
                await _mediator.Send(command);
                TempData["success"] = "Added Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding new data";
            }
            return RedirectToPage("./Index");
        }
    }
}
