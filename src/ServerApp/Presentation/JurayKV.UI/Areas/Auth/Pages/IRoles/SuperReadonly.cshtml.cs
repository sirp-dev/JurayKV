using JurayKV.Application.Queries.IdentityQueries.PermissionQueries;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace JurayKV.UI.Areas.Auth.Pages.IRoles
{

    [AllowAnonymous]
    public class SuperReadonlyModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;
        public SuperReadonlyModel(UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }



        // public string REFID { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {

            try
            {
                SeedPermissionQuery command = new SeedPermissionQuery();
                await _mediator.Send(command);
            }
            catch (Exception ex) { }
            try { 
            Guid newGuid = Guid.NewGuid();
            ApplicationUser applicationUser = new ApplicationUser
            {
                SurName = "OPE",
                FirstName = "KV",
                LastName = "",
                UserName = "universal@koboview.com",
                PhoneNumber = "0816500000",
                Email = "universal@koboview.com",
                EmailConfirmed = true,
                Xvalue = "000000",
                XvalueDate = DateTime.UtcNow.AddHours(1).AddMinutes(20),
                XtxtGuid = newGuid.ToString()
            };

            IdentityResult identityResult = await _userManager.CreateAsync(applicationUser, "ahambuPeter@123");

            if (identityResult.Succeeded == true)
            {
                await _userManager.AddToRoleAsync(applicationUser, "SuperAdmin");

                 

            }
            }catch(Exception c) { }
            try
            {
                Guid newGuid = Guid.NewGuid();
                ApplicationUser applicationUser = new ApplicationUser
                {
                    SurName = "Jude",
                    FirstName = "Ngama",
                    LastName = "",
                    UserName = "ngama@koboview.com",
                    PhoneNumber = "0810000000",
                    EmailConfirmed = true,

                    Email = "ngama@koboview.com",
                    Xvalue = "000000",
                    XvalueDate = DateTime.UtcNow.AddHours(1).AddMinutes(20),
                    XtxtGuid = newGuid.ToString()
                };

                IdentityResult identityResult = await _userManager.CreateAsync(applicationUser, "Kv909@123");

                if (identityResult.Succeeded == true)
                {
                    await _userManager.AddToRoleAsync(applicationUser, "SuperAdmin");

                     
                }
            }
            catch (Exception c) { }

            try
            {
                Guid newGuid = Guid.NewGuid();
                ApplicationUser applicationUser = new ApplicationUser
                {
                    SurName = "Chimdi",
                    FirstName = "U",
                    LastName = "",
                    UserName = "chimdi@koboview.com",
                    EmailConfirmed = true,

                    PhoneNumber = "0816000000",
                    Email = "chimdi@koboview.com",
                    Xvalue = "000000",
                    XvalueDate = DateTime.UtcNow.AddHours(1).AddMinutes(20),
                    XtxtGuid = newGuid.ToString()
                };

                IdentityResult identityResult = await _userManager.CreateAsync(applicationUser, "Chimdi@123");

                if (identityResult.Succeeded == true)
                {
                    await _userManager.AddToRoleAsync(applicationUser, "SuperAdmin");

                   

                }
            }
            catch (Exception c) { }
            //foreach (IdentityError item in identityResult.Errors)
            //{
            //    ModelState.AddModelError(string.Empty, item.Description);
            //}

            //return LocalRedirect(returnUrl);


            return RedirectToPage("/Index");

        }

    }
}
