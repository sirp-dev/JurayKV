using JurayKV.Application;
using JurayKV.Application.Commands.KvPointCommands;
using JurayKV.Application.Queries.KvPointQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.UI.Areas.KvMain.Pages.IPoints
{

    [Authorize(Policy = Constants.PointPolicy)]
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
            public Guid UserId { get; set; }
            public Guid IdentityKvAdId { get; set; }
            public EntityStatus Status { get; set; }
            public string PointHash { get; set; }
            public int Point { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier); // Get the user's ID claim

            if (userIdClaim != null)
            {
                string userId = userIdClaim.Value;
                Command.IdentityKvAdId = id;
                Command.UserId = Guid.Parse(userId);
                Command.PointHash = Guid.NewGuid().ToString();
                return Page();
            }
            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                CreateKvPointCommand command = new CreateKvPointCommand(Command.UserId, Command.IdentityKvAdId, Command.Status, Command.Point, Command.PointHash);
                Guid Result = await _mediator.Send(command);
                TempData["success"] = "Added Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding new bucket";
            }
            return RedirectToPage("./Index");
        }
    }
}
