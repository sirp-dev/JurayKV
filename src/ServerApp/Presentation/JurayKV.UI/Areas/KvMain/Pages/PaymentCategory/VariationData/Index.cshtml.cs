using JurayKV.Application;
using JurayKV.Application.Queries.ExchangeRateQueries;
using JurayKV.Domain.Aggregates.CategoryVariationAggregate;
using JurayKV.Domain.Aggregates.VariationAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.PaymentCategory.VariationData
{
    [Authorize(Policy = Constants.Permission)]

    public class IndexModel : PageModel
    {
        private readonly IVariationRepository _variationRepository;
        public IndexModel(IVariationRepository variationRepository)
        {
            _variationRepository = variationRepository;
        }


        public List<Variation> Variations = new List<Variation>();
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            Variations = await _variationRepository.GetAllListAsync();
             
            return Page();
        }
    }
}
