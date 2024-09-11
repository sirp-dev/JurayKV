using JurayKV.Application;
using JurayKV.Application.Commands.ExchangeRateCommands;
using JurayKV.Application.Infrastructures;
using JurayKV.Application.Queries.ExchangeRateQueries;
using JurayKV.Domain.Aggregates.CategoryVariationAggregate;
using JurayKV.Domain.Aggregates.ImageAggregate;
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
    public class UpdateModel : PageModel
    {
        private readonly IVariationRepository _variationRepository;
        private readonly ICategoryVariationRepository _categoryRepository;
        private readonly IStorageService _storage;

        public UpdateModel(IVariationRepository variationRepository, ICategoryVariationRepository categoryRepository, IStorageService storage)
        {
            _variationRepository = variationRepository;
            _categoryRepository = categoryRepository;
            _storage = storage;
        }


        [BindProperty]
        public Variation Variation { get; set; }
        public List<SelectListItem> ListCategories { get; set; }

        [BindProperty]
        public Guid Id { get; set; }
        [BindProperty]
        public IFormFile? imagefile { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                Variation = await _variationRepository.GetByIdAsync(id);
                //
                var categories = await _categoryRepository.GetAllListAsync();
                ListCategories = categories.Select(data =>
                   new SelectListItem
                   {
                       Value = data.Id.ToString(), // Assuming Id is an integer
                       Text = data.Name
                   }).ToList();
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
            var data = await _variationRepository.GetByIdAsync(Id);
            if(imagefile != null) { 
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
            }
            try
            {
                
                data.CategoryVariationId = Variation.CategoryVariationId;
                data.Code = Variation.Code;
                data.Active = Variation.Active;
                data.Amount = Variation.Amount;
                data.Name = Variation.Name;
                data.BillGateway = Variation.BillGateway;
                data.Tier = Variation.Tier;
                await _variationRepository.UpdateAsync(data);
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
