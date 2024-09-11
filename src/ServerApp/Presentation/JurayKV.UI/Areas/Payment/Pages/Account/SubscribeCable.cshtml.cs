using JurayKV.Application;
using JurayKV.Application.Queries.SettingQueries;
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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JurayKV.UI.Areas.Payment.Pages.Account
{
    [Authorize(Policy = Constants.Dashboard)]
    public class SubscribeCableModel : PageModel
    {

        private readonly IMediator _mediator;
        private readonly ICategoryVariationRepository _categoryRepository;
        private readonly IVariationRepository _variationRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public SubscribeCableModel(IMediator mediator, ICategoryVariationRepository categoryRepository, IVariationRepository variationRepository, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _categoryRepository = categoryRepository;
            _variationRepository = variationRepository;
            _userManager = userManager;
        }
        [BindProperty]
        public bool Verified { get; set; }

        [BindProperty]
        public VerifyRequest Request { get; set; }
        [BindProperty]
        public Guid CategoryId { get; set; }
        [BindProperty]
        public CategoryVariation CategoryVariation { get; set; }
        public IList<Variation> Variations { get; set; }
        public WalletDetailsDto WalletDetailsDto { get; set; }

        [BindProperty]
        public VerifyResponse VerifyResponseData { get; set; }

        [BindProperty]
        public CableTvRequest CableTvRequest { get; set; }

        [BindProperty]
        public string VariationId { get; set; }

        public List<SelectListItem> ListVariations { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                string userId = _userManager.GetUserId(HttpContext.User);
                CategoryVariation = await _categoryRepository.GetByIdAsync(id);
                GetWalletUserByIdQuery getwalletcommand = new GetWalletUserByIdQuery(Guid.Parse(userId));
                WalletDetailsDto = await _mediator.Send(getwalletcommand);
                Variations = await _variationRepository.GetByCategoryByActiveIdAsync(id);
                // Map CompanyDropdownListDto to SelectListItem
                ListVariations = Variations.Where(x => x.Active == true).Select(x =>
                    new SelectListItem
                    {
                        Value = x.Id.ToString(), // Assuming Id is an integer
                        Text = x.Name + " (₦" + x.Amount + ")"
                    }).ToList();
                //
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


                VerifyCustomerCommand verifyCommad = new VerifyCustomerCommand(Request.CustomerId, Request.ServiceId, Request.VariationId);
                VerifyResponseData = await _mediator.Send(verifyCommad);

                //create transaction and debit wallet
                if (VerifyResponseData.code == "success")
                {
                    TempData["success"] = "Successfuly";
                    Verified = true;
                    Variations = await _variationRepository.GetByCategoryByActiveIdAsync(CategoryId);
                    // Map CompanyDropdownListDto to SelectListItem
                    ListVariations = Variations.Where(x => x.Active == true).Select(x =>
                        new SelectListItem
                        {
                            Value = x.Id.ToString(), // Assuming Id is an integer
                            Text = x.Name + " (" + x.Amount + ")",
                        }).ToList();
                    //
                    return Page();
                }
                else if (VerifyResponseData.code == "processing")
                {
                    TempData["success"] = "Unable to Verify Meter Number";
                    return Page();
                }
                else if (VerifyResponseData.code == "failure")
                {
                    TempData["success"] = "Unable to Verify Meter Number";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "error. validation failed";
            }
            return RedirectToPage("/Account/Index", new { area = "User" });
        }
        public async Task<IActionResult> OnPostPayEE()
        {
            try
            {
                string userId = _userManager.GetUserId(HttpContext.User);
                var user = await _userManager.FindByIdAsync(userId);

                GetWalletUserByIdQuery getwalletcommand = new GetWalletUserByIdQuery(Guid.Parse(userId));
                WalletDetailsDto = await _mediator.Send(getwalletcommand);

                //get variation amount
                var variationData = await _variationRepository.GetByIdAsync(Guid.Parse(Request.VariationId));
                decimal Amount = Convert.ToDecimal(variationData.Amount);

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
                GetSettingDefaultQuery settingcommand = new GetSettingDefaultQuery();
                var setting = await _mediator.Send(settingcommand);
                if (setting.DisableTV == true)
                {
                    TempData["error"] = "Unable to process request. Try Again";

                    return Page();
                }
                if (user.Tier != variationData.Tier && user.Tier != Domain.Primitives.Enum.Tier.Tier2)
                {
                    if (user.Tier == Domain.Primitives.Enum.Tier.Tier1)
                        TempData["error"] = "Unable to process request. Upgrade to Tier 2";

                    return Page();
                }
                SubcribeCommand dataCommad = new SubcribeCommand(CableTvRequest.PhoneNumber, CableTvRequest.CustomerId,
                    CableTvRequest.VariationId, CableTvRequest.ServiceId, userId);
                CableTvResponse Result = await _mediator.Send(dataCommad);

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

                    TempData["error"] = "Network unavailable or Kindly Comfirm the number is correct and try again";

                    return Page();
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Network unavailable or Kindly Comfirm the number is correct and try again";
            }
            return RedirectToPage("/Account/Index", new { area = "User" });
        }

    }
}
