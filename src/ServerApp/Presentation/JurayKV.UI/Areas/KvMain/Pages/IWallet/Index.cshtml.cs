using JurayKV.Application;
using JurayKV.Application.Queries.WalletQueries;
using JurayKV.Application.Queries.IdentityQueries.PermissionQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JurayKV.UI.Areas.KvMain.Pages.IWallet
{
    [Authorize(Policy = Constants.Transaction)]
    public class IndexModel : PageModel
    {

        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public List<WalletDetailsDto> Wallets = new List<WalletDetailsDto>();
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            GetWalletListQuery command = new GetWalletListQuery();
            Wallets = await _mediator.Send(command);

            return Page();
        }
    }
}
