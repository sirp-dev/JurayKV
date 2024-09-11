using JurayKV.Application.Queries.IdentityKvAdQueries;
using JurayKV.Application.Queries.KvAdQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JurayKV.UI.Pages.ViewComponents
{
     public class CheckAdsAvailabilityViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;

        public CheckAdsAvailabilityViewComponent(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid userId, Guid kvAdsId)
        {

            CheckExistByUserIdAndAdsQuery command = new CheckExistByUserIdAndAdsQuery(userId, kvAdsId);
            bool KvAds = await _mediator.Send(command);
            return View(KvAds);
        }
    }
}
