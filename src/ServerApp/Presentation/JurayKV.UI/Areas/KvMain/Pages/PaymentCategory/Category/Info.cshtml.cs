using JurayKV.Application;
using JurayKV.Application.Queries.ExchangeRateQueries;
using JurayKV.Domain.Aggregates.CategoryVariationAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.PaymentCategory.Category
{

    [Authorize(Policy = Constants.AdvertPolicy)]
    public class InfoModel : PageModel
    {

        private readonly ICategoryVariationRepository _categoryRepository;
        public InfoModel(ICategoryVariationRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        [BindProperty]
        public CategoryVariation CategoryVariation { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                CategoryVariation = await _categoryRepository.GetByIdAsync(id);

                return Page();
            }
            catch (Exception ex)
            {
                TempData["error"] = "unable to fetch bucket";
                return RedirectToPage("/Index");
            }
        }


    }
}
