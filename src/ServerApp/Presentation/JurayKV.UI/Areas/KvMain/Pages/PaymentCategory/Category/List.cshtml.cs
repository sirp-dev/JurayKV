using JurayKV.Application;
using JurayKV.Domain.Aggregates.VariationAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.PaymentCategory.Category
{
      [Authorize(Policy = Constants.AdminPolicy)]

    public class ListModel : PageModel
    {
        private readonly IVariationRepository _variationRepository;
        public ListModel(IVariationRepository variationRepository)
        {
            _variationRepository = variationRepository;
        }


        public List<Variation> Variations = new List<Variation>();
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Variations = await _variationRepository.GetByCategoryIdAsync(id);

            return Page();
        }
    }
}
