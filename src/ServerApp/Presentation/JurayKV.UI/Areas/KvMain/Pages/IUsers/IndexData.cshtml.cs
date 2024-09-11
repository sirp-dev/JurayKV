using JurayKV.Application;
using JurayKV.Application.Queries.UserManagerQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.UI.Areas.KvMain.Pages.IUsers
{
      [Authorize(Policy = Constants.UsersManagerPolicy)]
    public class IndexDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IMediator _mediator;
        public IndexDataModel(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }
        public string Title { get; set; }
        public List<UserManagerListDto> UserManagers = new List<UserManagerListDto>();

        public bool ShowAll { get; set; }
        public bool ShowWithoutDate { get; set; }
        public int All { get; set; }
        public int ActiveOnly { get; set; }
        public int ActiveWhatsapp { get; set; }
        public int Suspended { get; set; }
        public int Disabled { get; set; }

        public int DayAll { get; set; }
        public int DayActiveOnly { get; set; }
        public int DayActiveWhatsapp { get; set; }
        public int DaySuspended { get; set; }
        public int DayDisabled { get; set; }

        public int PageSize { get; set; } = 100; // Set a default value
        public int PageNumber { get; set; } = 1; // Set a default value
        public int TotalPages { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Searchstring { get; set; }
        public AccountStatus Status { get; set; }

        public string? DistintPhone { get; set; }
        public int DistintPhoneCount { get; set; }
        public int DistintPhoneCountActive { get; set; }
        public string? DistintEmail { get; set; }
        public int DistintEmailCount { get; set; }
        public int DistintEmailCountActive { get; set; }
        public int SortOrder = 0;
        public async Task<IActionResult> OnGetAsync(int? pageNumber, int? pageSize, AccountStatus status = AccountStatus.NotDefind, bool all = false, DateTime? startdate = null, DateTime? enddate = null, string? searchstring = null, int sortOrder = 0)
        {
            Searchstring = searchstring;
            PageNumber = pageNumber ?? PageNumber;
            if (all)
            {
                GetUserByStatusListQuery command = new GetUserByStatusListQuery(Domain.Primitives.Enum.AccountStatus.NotDefind, DateTime.MinValue, DateTime.MaxValue, PageSize, PageNumber, searchstring, sortOrder);
                var getUserManagers = await _mediator.Send(command);
                UserManagers = getUserManagers.UserManagerListDto.ToList();
                TotalPages = (int)Math.Ceiling((double)getUserManagers.TotalCount / PageSize);

                DistintEmail = getUserManagers.DistintEmail;
                DistintEmailCount = getUserManagers.DistintEmailCount;
                DistintPhone = getUserManagers.DistintPhone;
                DistintPhoneCount = getUserManagers.DistintPhoneCount;
                DistintPhoneCountActive = getUserManagers.DistintPhoneCountActive;
                DistintEmailCountActive = getUserManagers.DistintEmailCountActive;
                Title = "ALL USERS";
                ShowAll = true;
            }
            else
            {
                if (startdate == DateTime.MinValue)
                {
                    ShowWithoutDate = true;
                    StartDate = null;
                    EndDate = null;
                }
                else
                {

                    StartDate = startdate;
                    EndDate = enddate;
                }
                // 
                GetUserByStatusListQuery command = new GetUserByStatusListQuery(status, StartDate, EndDate, PageSize, PageNumber, searchstring, sortOrder);
                var getUserManagers = await _mediator.Send(command);
                UserManagers = getUserManagers.UserManagerListDto.ToList();
                TotalPages = (int)Math.Ceiling((double)getUserManagers.TotalCount / PageSize);

                DistintEmail = getUserManagers.DistintEmail;
                DistintEmailCount = getUserManagers.DistintEmailCount;
                DistintPhone = getUserManagers.DistintPhone;
                DistintPhoneCount = getUserManagers.DistintPhoneCount;
                DistintPhoneCountActive = getUserManagers.DistintPhoneCountActive;
                DistintEmailCountActive = getUserManagers.DistintEmailCountActive;
                if (status == AccountStatus.NotDefind)
                {
                    Title = "ACTIVE USERS";
                }
                else
                {
                    Title = $"{status.ToString().ToUpper()} USERS";
                }
            }
            Status = status;
            ///
            DayAll = UserManagers.Count();
            DayActiveOnly = UserManagers.Where(x => x.AccountStatus == Domain.Primitives.Enum.AccountStatus.Active).Count();
            DaySuspended = UserManagers.Where(x => x.AccountStatus == Domain.Primitives.Enum.AccountStatus.Suspended).Count();
            DayDisabled = UserManagers.Where(x => x.AccountStatus == Domain.Primitives.Enum.AccountStatus.Disabled).Count();
            ///

            var entity = _userManager.Users.AsQueryable();

            All = entity.Count();
            ActiveOnly = entity.Where(x => x.AccountStatus == Domain.Primitives.Enum.AccountStatus.Active).Count();
            Suspended = entity.Where(x => x.AccountStatus == Domain.Primitives.Enum.AccountStatus.Suspended).Count();
            Disabled = entity.Where(x => x.AccountStatus == Domain.Primitives.Enum.AccountStatus.Disabled).Count();
            return Page();
        }
    }
}
