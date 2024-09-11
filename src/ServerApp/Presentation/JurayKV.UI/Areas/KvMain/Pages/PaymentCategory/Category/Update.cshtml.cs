using JurayKV.Application;
using JurayKV.Application.Commands.ExchangeRateCommands;
using JurayKV.Application.Infrastructures;
using JurayKV.Application.Queries.ExchangeRateQueries;
using JurayKV.Domain.Aggregates.CategoryVariationAggregate;
using JurayKV.Domain.Aggregates.VariationAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.PaymentCategory.Category
{
    [Authorize(Policy = Constants.AdvertPolicy)]
    public class UpdateModel : PageModel
    {

        private readonly ICategoryVariationRepository _categoryRepository;
        private readonly IStorageService _storage;

        public UpdateModel(ICategoryVariationRepository categoryRepository, IStorageService storage)
        {
            _categoryRepository = categoryRepository;
            _storage = storage;
        }

        [BindProperty]
        public CategoryVariation CategoryVariation { get; set; }

        [BindProperty]
        public Guid Id { get; set; }
        [BindProperty]
        public IFormFile? imagefile { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                CategoryVariation = await _categoryRepository.GetByIdAsync(id);
                //
                return Page();
            }
            catch (Exception ex)
            {
                TempData["error"] = "unable to fetch Ads";
                return RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var data = await _categoryRepository.GetByIdAsync(Id);

            try
            {

                var xresult = await _storage.MainUploadFileReturnUrlAsync("", imagefile);
                // 
                if (xresult.Message.Contains("200"))
                {
                    data.Url = xresult.Url;
                    data.Key = xresult.Key;
                }

            }
            catch (Exception c)
            {

            }
            try
            {
                data.VariationType = CategoryVariation.VariationType;
                data.Active = CategoryVariation.Active;
                data.Name   = CategoryVariation.Name;
                data.Charge = CategoryVariation.Charge;
                data.BillGateway = CategoryVariation.BillGateway;
                data.Tier = CategoryVariation.Tier;
                await _categoryRepository.UpdateAsync(data);
                TempData["success"] = "Updated Successfuly";

            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding new bucket";
            }
            return RedirectToPage("./Index");

        }
    }
}
