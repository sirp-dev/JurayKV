using JurayKV.Application;
using JurayKV.Application.Commands.ExchangeRateCommands;
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
    public class RemoveModel : PageModel
    {

        private readonly IVariationRepository _variationRepository;
        public RemoveModel(IVariationRepository variationRepository)
        {
            _variationRepository = variationRepository;
        }


        [BindProperty]
        public ExchangeRateDetailsDto UpdateExchangeRate { get; set; }

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
                TempData["error"] = "unable to fetch bucket";
                return RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {

                var data = await _variationRepository.GetByIdAsync(Variation.Id);
                await _variationRepository.DeleteAsync(data);
                TempData["success"] = "Deleted Successfuly";
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. removing new variation";
            }
            return RedirectToPage("./Index");
        }
    }
}
