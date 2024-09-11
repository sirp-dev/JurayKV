using Azure.Core;
using JurayKV.Application;
using JurayKV.Application.Commands.ExchangeRateCommands;
using JurayKV.Application.Infrastructures;
using JurayKV.Domain.Aggregates.CategoryVariationAggregate;
using JurayKV.Domain.Aggregates.VariationAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client.Extensions.Msal;

namespace JurayKV.UI.Areas.KvMain.Pages.PaymentCategory.VariationData
{
    [Authorize(Policy = Constants.Permission)]
    public class AddModel : PageModel
    {

        private readonly IVariationRepository _variationRepository;
        private readonly ICategoryVariationRepository _categoryRepository;
        private readonly IStorageService _storage;

        public AddModel(IVariationRepository variationRepository, ICategoryVariationRepository categoryRepository, IStorageService storage)
        {
            _variationRepository = variationRepository;
            _categoryRepository = categoryRepository;
            _storage = storage;
        }

        [BindProperty]
        public Variation Variation { get; set; }
        public List<SelectListItem> ListCategories { get; set; }

        [BindProperty]
        public IFormFile? imagefile { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var categories = await _categoryRepository.GetAllListAsync();
            ListCategories = categories.Select(data =>
               new SelectListItem
               {
                   Value = data.Id.ToString(), // Assuming Id is an integer
                   Text = data.Name
               }).ToList();
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
                    Variation.Url = xresult.Url;
                    Variation.Key = xresult.Key;
                }

            }
            catch (Exception c)
            {

            }
            try
            {
                await _variationRepository.InsertAsync(Variation);
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
