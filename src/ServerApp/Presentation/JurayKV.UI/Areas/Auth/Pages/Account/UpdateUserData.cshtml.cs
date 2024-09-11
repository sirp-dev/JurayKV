using JurayKV.Application.Commands.UserManagerCommands;
using JurayKV.Application.Infrastructures;
using JurayKV.Application.Queries.StateLgaQueries;
using JurayKV.Application.Queries.UserManagerQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JurayKV.UI.Areas.Auth.Pages.Account
{
    public class UpdateUserDataModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStorageService _storage;

        public UpdateUserDataModel(IMediator mediator, UserManager<ApplicationUser> userManager, IStorageService storage)
        {
            _mediator = mediator;
            _userManager = userManager;
            _storage = storage;
        }

        [BindProperty]
        public UserManagerUpdateDto UpdateUserManager { get; set; }

        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> ListStates { get; set; }
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> ListStatesOfOrigin { get; set; }
        public IFormFile IDcard { get; set; }
        public IFormFile Passport { get; set; }
        public string IDD {  get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var data = TempData["id"];
            if (data == null)
            {
                return RedirectToPage("/Index");
            }
            try
            {
                GetUserManagerByIdQuery command = new GetUserManagerByIdQuery(Guid.Parse(data.ToString()));
                var user = await _mediator.Send(command);

                if (user != null)
                {
                    UpdateUserManager = new UserManagerUpdateDto
                    {

                        Surname = user.Surname,
                        Firstname = user.Firstname,
                        Lastname = user.Lastname,
                        DateOfBirth = user.DateOfBirth,
                        StateOfOrigin = user.StateOfOrigin,
                        LGA_Of_Origin = user.LGA_Of_Origin,
                        About = user.About,
                        AlternativePhone = user.AlternativePhone,
                        Address = user.Address,
                        State = user.State,
                        LGA = user.LGA,
                        Occupation = user.Occupation,

                    };
                    IDD = user.Id.ToString();
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
                    return Page();
                }

                TempData["error"] = "unable to fetch data";
                return RedirectToPage("/Index");
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
                var update = await _userManager.FindByIdAsync(UpdateUserManager.Id);
                try
                {

                    var xresult = await _storage.MainUploadFileReturnUrlAsync("", IDcard);
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        update.IDCardUrl = xresult.Url;
                        update.IDCardKey = xresult.Key;
                    }
                    
                }
                catch (Exception c)
                {
                 }
                try
                {

                    var xresult = await _storage.MainUploadFileReturnUrlAsync("", Passport);
                    // 
                    if (xresult.Message.Contains("200"))
                    {
                        update.PassportUrl = xresult.Url;
                        update.PassportKey = xresult.Key;
                    }
                    else
                    {
                     }

                }
                catch (Exception c)
                {
                 }
                update.SurName = UpdateUserManager.Surname;
                update.FirstName = UpdateUserManager.Firstname;
                update.LastName = UpdateUserManager.Lastname;
                update.State = UpdateUserManager.State;
                update.LGA = UpdateUserManager.LGA;
                update.Address = UpdateUserManager.Address;
                update.DateOfBirth = UpdateUserManager.DateOfBirth;
                update.StateOfOrigin = UpdateUserManager.StateOfOrigin;
                update.LGA_Of_Origin = UpdateUserManager.LGA_Of_Origin;
                update.About = UpdateUserManager.About;
                update.AlternativePhone = UpdateUserManager.AlternativePhone;
                update.Occupation = UpdateUserManager.Occupation;
                await _userManager.UpdateAsync(update);

                TempData["success"] = "Updated Successfuly";
                return RedirectToPage("./Locked", new { id = update.Id });

            }
            catch (Exception ex)
            {
                TempData["error"] = "error. adding updating";
            }
            return RedirectToPage("./Index");
        }


        public async Task<JsonResult> OnGetLGAs(Guid stateId)
        {
            LgaQuery lgaCommand = new LgaQuery(stateId);
            var listStates = await _mediator.Send(lgaCommand);

            var lgas = listStates.Select(l => new { value = l.LGA, text = l.LGA }).ToList();
            return new JsonResult(lgas);
        }


        public async Task<JsonResult> OnGetLGAsOfOrigin(Guid statexId)
        {
            LgaQuery lgaCommand = new LgaQuery(statexId);
            var listStates = await _mediator.Send(lgaCommand);

            var lgas = listStates.Select(l => new { value = l.LGA, text = l.LGA }).ToList();
            return new JsonResult(lgas);
        }

    }
}
