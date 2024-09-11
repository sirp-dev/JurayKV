using JurayKV.Application;
using JurayKV.Application.Commands.ExchangeRateCommands;
using JurayKV.Application.Infrastructures;
using JurayKV.Domain.Aggregates.CategoryVariationAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.PaymentCategory.Category
{
    [Authorize(Policy = Constants.AdvertPolicy)]
    public class AddModel : PageModel
    {

        private readonly ICategoryVariationRepository _categoryRepository;
        private readonly IStorageService _storage;

        public AddModel(ICategoryVariationRepository categoryRepository, IStorageService storage)
        {
            _categoryRepository = categoryRepository;
            _storage = storage;
        }

        [BindProperty]
        public CategoryVariation CategoryVariation { get; set; }


        [BindProperty]
        public IFormFile? imagefile { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {

                var xresult = await _storage.MainUploadFileReturnUrlAsync("", imagefile);
                // 
                if (xresult.Message.Contains("200"))
                {
                    CategoryVariation.Url = xresult.Url;
                    CategoryVariation.Key = xresult.Key;
                }

            }
            catch (Exception c)
            {

            }
            try
            {
                 await _categoryRepository.InsertAsync(CategoryVariation);
                TempData["success"] = "Added Successfuly";

            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding new bucket";
            }
            return RedirectToPage("./Index");
        }
    }
}
