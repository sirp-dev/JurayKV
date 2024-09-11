using JurayKV.Application;
using JurayKV.Application.Queries.SliderQueries;
using JurayKV.Application.Queries.UserMessageQueries;
using JurayKV.Domain.Aggregates.UserMessageAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IUserMessages
{
    [Authorize(Policy = Constants.AdminPolicy)]
    public class IndexModel : PageModel
    {

        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<UserMessage> Messages = new List<UserMessage>();
        public async Task<IActionResult> OnGetAsync()
        {
            //GetMessageListQuery command = new GetMessageListQuery();
            //Messages = await _mediator.Send(command);
            GetMessageByIdQuery command = new GetMessageByIdQuery(Guid.NewGuid());
            var UserMessage = await _mediator.Send(command);
            return Page();
        }
    }
}
