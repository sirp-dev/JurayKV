using JurayKV.Application.Commands.BucketCommands;
using JurayKV.Application.Commands.ClearCommands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.Auth.Pages.Account
{
    public class ClearModel : PageModel
    {
        private readonly IMediator _mediator;
        public ClearModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async void OnGet()
        {
            ClearAllCatchCommand command = new ClearAllCatchCommand();
            await _mediator.Send(command);
        }
    }
}
