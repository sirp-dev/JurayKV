using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Services
{
    public class BasePageModel : PageModel
    {
        private ISender _mediator = null;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
