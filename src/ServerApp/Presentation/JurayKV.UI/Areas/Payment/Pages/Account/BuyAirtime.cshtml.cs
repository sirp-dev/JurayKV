using Amazon.Runtime.Internal;
using Azure.Core;
using JurayKV.Application;
using JurayKV.Application.Commands.ExchangeRateCommands;
using JurayKV.Application.Queries.SettingQueries;
using JurayKV.Application.Queries.TransactionQueries;
using JurayKV.Application.Queries.WalletQueries;
using JurayKV.Application.VtuServices;
using JurayKV.Domain.Aggregates.CategoryVariationAggregate;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Domain.Aggregates.VariationAggregate;
using JurayKV.Infrastructure.VTU.RequestDto;
using JurayKV.Infrastructure.VTU.ResponseDto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace JurayKV.UI.Areas.Payment.Pages.Account
{
    [Authorize(Policy = Constants.Dashboard)]
    public class BuyAirtimeModel : PageModel
    {

        private readonly IMediator _mediator;
        private readonly ICategoryVariationRepository _categoryRepository;
        private readonly IVariationRepository _variationRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public BuyAirtimeModel(IMediator mediator, ICategoryVariationRepository categoryRepository, IVariationRepository variationRepository, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _categoryRepository = categoryRepository;
            _variationRepository = variationRepository;
            _userManager = userManager;
        }

        [BindProperty]
        public AirtimeRequest Request { get; set; }
        [BindProperty]
        public Guid CategoryId { get; set; }
        [BindProperty]
        public CategoryVariation CategoryVariation { get; set; }
        public IList<Variation> Variations { get; set; }
        public WalletDetailsDto WalletDetailsDto { get; set; }
        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                string userId = _userManager.GetUserId(HttpContext.User);
                CategoryVariation = await _categoryRepository.GetByIdAsync(id);
                GetWalletUserByIdQuery getwalletcommand = new GetWalletUserByIdQuery(Guid.Parse(userId));
                WalletDetailsDto = await _mediator.Send(getwalletcommand);
                //Variations = await _variationRepository.GetByCategoryByActiveIdAsync(id);
                return Page();
            }
            catch (Exception ex)
            {
                TempData["error"] = "unable to fetch data";
                return RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
           
            try
            {
                string userId = _userManager.GetUserId(HttpContext.User);
                CategoryVariation = await _categoryRepository.GetByIdAsync(CategoryId);
                GetWalletUserByIdQuery getwalletcommand = new GetWalletUserByIdQuery(Guid.Parse(userId));
                WalletDetailsDto = await _mediator.Send(getwalletcommand);
                decimal Amount = 0;

                GetSettingDefaultQuery settingcommand = new GetSettingDefaultQuery();
                var setting = await _mediator.Send(settingcommand);
                if (setting.DisableAirtime == true)
                {
                    TempData["error"] = "Unable to process request. Try Again";

                    return Page();
                }
                try
                {
                    Amount = Convert.ToDecimal(Request.Amount); 
                    if(Amount < 20)
                    {
                        TempData["error"] = "Minimum amount is 20";
                        return Page();
                    }
                }
                catch (Exception c)
                {
                    
                    TempData["error"] = "Invalid Amount";
                    return Page();
                }
                if (WalletDetailsDto != null)
                {
                    if (WalletDetailsDto.Amount < Amount)
                    {
                         
                        TempData["error"] = "Insufficient Balance";
                        return Page();
                    }

                }
                else
                {
                    TempData["error"] = "Unable to process request";

                    return Page();
                }
                //check if they recharged above the limit
                
                 AirtimeCommad airtimeCommad = new AirtimeCommad(Request.PhoneNumber, Request.Network, Request.Amount, userId);
                AirtimeResponse Result = await _mediator.Send(airtimeCommad);

                //create transaction and debit wallet
                if (Result.code == "success")
                {
                    TempData["success"] = "Successfuly";
                }
                else if (Result.code == "processing")
                {
                    TempData["success"] = "Processing";

                }
                else if (Result.code == "failure")
                {
                    if (Result.message.Contains("DUPLICATE"))
                    {
                        TempData["error"] = "DUPLICATE ORDER. Please wait for 3 minutes before placing another airtime order of the same amount to the same phone number.";

                    }
                    else { 
                    TempData["error"] = "Network unavailable or Kindly Comfirm the number is correct and try again";
                    }
                    return Page();
                }
                else if (Result.code == "limit")
                {
                    
                        TempData["error"] = "Kindly upgrade to Tier 2 from your profile. Thanks";
                        TempData["response"] = "Kindly upgrade to Tier 2. Thanks";

                    return Page();
                }
                
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. validation failed";
            }
            return RedirectToPage("/Account/Index", new { area = "User" });
        }
    }
}
