using JurayKV.Application;
using JurayKV.Application.Queries.TransactionQueries;
using JurayKV.Application.Queries.UserManagerQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.UI.Areas.KvMain.Pages.ITransactions
{
    [Authorize(Policy = Constants.UsersManagerPolicy)]
    public class TransactionAnalysisModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IMediator _mediator;
        public TransactionAnalysisModel(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }
        public string Title { get; set; }
        public List<UserManagerListDto> UserManagers = new List<UserManagerListDto>();
        public List<ListTransactionDto> ListUsers { get; set; }
        public ListTransactionDto Dashboard { get; set; }

        public int PageSize { get; set; } = 100; // Set a default value
        public int PageNumber { get; set; } = 1; // Set a default value
        public int TotalPages { get; set; }

        public string Searchstring { get; set; }
        public int SortOrder = 0;


        public decimal XTotalPoints { get; set; }
        public decimal XTotalReferrals { get; set; }
        public decimal XTotalDebit { get; set; }
        public decimal XWalletBalance { get; set; }

        public async Task<IActionResult> OnGetAsync(int? pageNumber, int? pageSize, string? searchstring = null, int sortOrder = 0)
        {
            Searchstring = searchstring;
            PageNumber = pageNumber ?? PageNumber;

            ListTransactionByUserAnalysis command = new ListTransactionByUserAnalysis(PageSize, PageNumber, searchstring, sortOrder);
            var datareturn = await _mediator.Send(command);
            ListUsers = datareturn;
            //
            int countAll = datareturn.FirstOrDefault().TotalInList;


            TotalPages = (int)Math.Ceiling((double)countAll / PageSize);


            try
            {
                GetUserTransactionsSummaryQuery getUserTransactionsSummaryQuery = new GetUserTransactionsSummaryQuery();
                Dashboard = await _mediator.Send(getUserTransactionsSummaryQuery);
            }
            catch(Exception ex) { }


            return Page();
        }
    }
}
