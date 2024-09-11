using JurayKV.Application;
using JurayKV.Application.Commands.UserManagerCommands;
using JurayKV.Application.Queries.StateLgaQueries;
using JurayKV.Application.Queries.UserManagerQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JurayKV.UI.Areas.User.Pages.Account
{
    [Authorize(Policy = Constants.Dashboard)]

    public class CSAModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public CSAModel(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }
        public List<UserManagerListDto> UserData { get; set; }
        public string Phonenumber { get; set; }
        public List<SelectListItem> ListStates { get; set; }
        [BindProperty]
        public UserUpgradeDto UserUpgradeDto { get; set; }

        public bool UserRequest {  get; set; }
        public string UserRequestMessage {  get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            var userdata = await _userManager.FindByIdAsync(userId);
            //UserUpgradeDto.About = userdata.About ?? "";
            //UserUpgradeDto.AlternativePhone = userdata.AlternativePhone ?? "";
            //UserUpgradeDto.Address = userdata.Address ?? "";
            //UserUpgradeDto.FbHandle = userdata.FbHandle ?? "";
            //UserUpgradeDto.InstagramHandle = userdata.InstagramHandle ?? "";
            //UserUpgradeDto.TwitterHandle = userdata.TwitterHandle ?? "";
            //UserUpgradeDto.TiktokHandle = userdata.TiktokHandle ?? "";
            UserRequest = userdata.CsaRequest;
            UserRequestMessage = userdata.ResponseOnCsaRequest;
            //
            StateQuery stateCommand = new StateQuery();
             var listStates = await _mediator.Send(stateCommand);

            ListStates = listStates.Select(data =>
               new SelectListItem
               {
                   Value = data.Id.ToString(), // Assuming Id is an integer
                   Text = data.State
               }).ToList();

            return Page();
        }
        [BindProperty]
        public IFormFile? passportfile { get; set; }
        [BindProperty]
        public IFormFile? iDcardfile { get; set; }
        public ResponseCsaUpgrade ResponseCsaUpgrade { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            CsaUpdateUserManagerCommand command = new CsaUpdateUserManagerCommand(Guid.Parse(userId), UserUpgradeDto, iDcardfile, passportfile);
            ResponseCsaUpgrade = await _mediator.Send(command);
            return RedirectToPage();
        }



        public async Task<JsonResult> OnGetLGAs(Guid stateId)
        {
            LgaQuery lgaCommand = new LgaQuery(stateId);
            var listStates = await _mediator.Send(lgaCommand);

            var lgas = listStates.Select(l => new { value = l.LGA, text = l.LGA }).ToList();
            return new JsonResult(lgas); 
        }
    }
}
