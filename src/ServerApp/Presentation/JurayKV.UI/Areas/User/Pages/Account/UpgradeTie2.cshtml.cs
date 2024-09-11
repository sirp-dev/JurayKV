using JurayKV.Application;
using JurayKV.Application.Commands.UserManagerCommands;
using JurayKV.Application.Queries.StateLgaQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using static JurayKV.Domain.Primitives.Enum;

namespace JurayKV.UI.Areas.User.Pages.Account
{
    [Authorize(Policy = Constants.Dashboard)]

    public class UpgradeTie2Model : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpgradeTie2Model(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }
        
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> ListStates { get; set; }
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> ListStatesOfOrigin { get; set; }

        [BindProperty]
        public TieTwoRequestDto TieTwoRequestDto { get; set; } = new TieTwoRequestDto();

        public TieRequestStatus Tie2Request { get; set; }
        public string ResponseOnTieRequest { get; set; }
        public string Passport { get; set; }
        public string IdCard { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            var userdata = await _userManager.FindByIdAsync(userId);
            Tie2Request = userdata.Tie2Request;
            ResponseOnTieRequest = userdata.ResponseOnTieRequest;
            //
            StateQuery stateCommand = new StateQuery();
            var listStates = await _mediator.Send(stateCommand);

            ListStates = listStates.Select(data =>
               new SelectListItem
               {
                   Value = data.Id.ToString(), // Assuming Id is an integer
                   Text = data.State
               }).ToList();

            ListStatesOfOrigin = listStates.Select(data =>
                     new SelectListItem
                     {
                         Value = data.Id.ToString(), // Assuming Id is an integer
                         Text = data.State
                     }).ToList();
            TieTwoRequestDto.State = userdata.State;
            TieTwoRequestDto.LGA   = userdata.LGA;
            TieTwoRequestDto.Address = userdata.Address;
            TieTwoRequestDto.Surname = userdata.SurName;
            TieTwoRequestDto.Firstname = userdata.FirstName;
            TieTwoRequestDto.Lastname = userdata.LastName;
            TieTwoRequestDto.Occupation = userdata.Occupation;
            TieTwoRequestDto.About = userdata.About;
            Passport = userdata.PassportUrl;
            IdCard = userdata.IDCardUrl;
            return Page();
        }
        [BindProperty]
        public IFormFile passportfile { get; set; }
        [BindProperty]
        public IFormFile iDcardfile { get; set; }
        public ResponseCsaUpgrade ResponseCsaUpgrade { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            TieTwoRequestUserManagerCommand command = new TieTwoRequestUserManagerCommand(Guid.Parse(userId), TieTwoRequestDto, iDcardfile, passportfile);
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
