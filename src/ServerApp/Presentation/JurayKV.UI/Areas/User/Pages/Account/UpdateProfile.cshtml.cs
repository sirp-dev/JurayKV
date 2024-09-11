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

    public class UpdateProfileModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateProfileModel(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> ListStates { get; set; }
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> ListStatesOfOrigin { get; set; }

        [BindProperty]
        public UpdateProfileDto UpdateProfileDto { get; set; } = new UpdateProfileDto();

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
           
            UpdateProfileDto.State = userdata.State;
            UpdateProfileDto.LGA = userdata.LGA;
            UpdateProfileDto.Address = userdata.Address;
            UpdateProfileDto.Surname = userdata.SurName;
            UpdateProfileDto.Firstname = userdata.FirstName;
            UpdateProfileDto.Lastname = userdata.LastName; 
            UpdateProfileDto.DateOfBirth = userdata.DateOfBirth;
            Passport = userdata.PassportUrl;
            IdCard = userdata.IDCardUrl;
            return Page();
        }
        [BindProperty]
        public IFormFile passportfile { get; set; }
        [BindProperty]
        public IFormFile iDcardfile { get; set; } 
        public async Task<IActionResult> OnPostAsync()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            UpdateProfileCommand command = new UpdateProfileCommand(Guid.Parse(userId), UpdateProfileDto);
            bool response = await _mediator.Send(command);
            if (response)
            {
                TempData["msg"] = "Update";
            }
            return RedirectToPage("./Profile");
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
