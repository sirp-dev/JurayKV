using JurayKV.Application;
using JurayKV.Application.Queries.ExchangeRateQueries;
using JurayKV.Domain.Aggregates.CategoryVariationAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.PaymentCategory.Category
{
    [Authorize(Policy = Constants.AdminPolicy)]

    public class IndexModel : PageModel
    {
        private readonly ICategoryVariationRepository _categoryRepository;
        public IndexModel(ICategoryVariationRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public List<CategoryVariation> CategoryVariation = new List<CategoryVariation>();
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            CategoryVariation = await _categoryRepository.GetAllListAsync();

            return Page();
        }
    }
}
