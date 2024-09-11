using JurayKV.Application.Queries.KvAdQueries;
using JurayKV.Domain.Aggregates.KvAdAggregate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JurayKV.UI.Pages.ViewComponents
{
    public class AdsViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public AdsViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            GetKvAdListQuery command = new GetKvAdListQuery();
           var KvAds = await _mediator.Send(command);
            return View(KvAds.OrderByDescending(x=>x.CreatedAtUtc).Take(7).ToList());
        }
    }
}
