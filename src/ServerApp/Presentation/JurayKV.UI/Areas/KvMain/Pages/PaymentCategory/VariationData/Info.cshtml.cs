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
    public class InfoModel : PageModel
    {

        private readonly IVariationRepository _variationRepository;
        public InfoModel(IVariationRepository variationRepository)
        {
            _variationRepository = variationRepository;
        }


        [BindProperty]
        public Variation Variation { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                Variation = await _variationRepository.GetByIdAsync(id);

                return Page();
            }
            catch (Exception ex)
            {
                TempData["error"] = "unable to fetch variation";
                return RedirectToPage("/Index");
            }
        }


    }
}
