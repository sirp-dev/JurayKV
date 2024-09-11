// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using JurayKV.Application.Commands.BucketCommands;
using JurayKV.Application.Commands.UserManagerCommands;
using JurayKV.Application.Infrastructures;
using JurayKV.Application.Validation;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using JurayKV.Persistence.RelationalDB.Migrations;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;

namespace JurayKV.UI.Areas.Auth.Pages.Account
{
    public class RegisterModel : PageModel
    {

        private readonly ILogger<RegisterModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IMediator _mediator;

        public RegisterModel(UserManager<ApplicationUser> userManager, IExceptionLogger exceptionLogger, ILogger<RegisterModel> logger, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, IMediator mediator)
        {
            _userManager = userManager;
            _exceptionLogger = exceptionLogger;
            _logger = logger;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _mediator = mediator;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>B to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {

            [Required]
            [MaxLength(100, ErrorMessage = "The {1} can't be more than {1} characters long.")]
            public string Surname { get; set; }

            [Required]
            [MaxLength(100, ErrorMessage = "The {1} can't be more than {1} characters long.")]
            public string FirstName { get; set; }
            [Required]
            [ValidatePhoneNumber]
            [MinLength(8, ErrorMessage = "The {1} can't be less than {1} characters long.")]
            public string PhoneNumber { get; set; }

            [Required]
            [MinLength(4, ErrorMessage = "The {0} must be at least {1} characters long.")]
            [MaxLength(50, ErrorMessage = "The {1} can't be more than {1} characters long.")]
            [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
            public string Email { get; set; }

            [Required]
            [MinLength(6, ErrorMessage = "The {0} must be at least {1} characters long.")]
            [MaxLength(20, ErrorMessage = "The {1} can't be more than {1} characters long.")]
            public string Password { get; set; }

            [Required]
            [Compare(nameof(Password), ErrorMessage = "Confirm password does match with password.")]
            public string ConfirmPassword { get; set; }

            [Required] 
            public string State { get; set; }

            [Required] 
            public string LGA { get; set; }


            [Required]
            public string Address { get; set; }
        }

        [BindProperty]
        public string RefPhone { get; set; }
        [BindProperty]
        public string RefName { get; set; }

        public bool CheckVerified { get; set; }
        public async Task OnGetAsync(string returnUrl = null, string refx = null)
        {
            ReturnUrl = returnUrl;
            if (refx != null)
            {
                try
                {
                    string last10DigitsPhoneNumber1 = refx.Substring(Math.Max(0, refx.Length - 10));

                    var userref = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber.Contains(refx));
                    if (userref != null)
                    {
                        RefName = userref.SurName + " " + userref.FirstName;
                        RefPhone = userref.PhoneNumber;

                        //check ref verified count.
                        //CheckUserRefVerificationCommand checkcommand = new CheckUserRefVerificationCommand(refx);
                        //CheckVerified = await _mediator.Send(checkcommand);
                    }
                }
                catch { }
            }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    CreateUserDto create = new CreateUserDto();
                    create.Surname = Input.Surname;
                    create.Firstname = Input.FirstName;
                    create.Email = Input.Email;
                    create.Password = Input.Password;
                    create.PhoneNumber = Input.PhoneNumber;
                    create.Role = "User";
                    create.RefPhone = RefPhone;
                    create.State = Input.State;
                    create.LGA = Input.LGA;
                    create.Address = Input.Address;
                    CreateUserManagerCommand command = new CreateUserManagerCommand(create);
                    ResponseCreateUserDto Result = await _mediator.Send(command);
                    if (Result.Succeeded == false)
                    {
                        Input.Password = null;
                        Input.ConfirmPassword = null;
                        RefName = RefName;
                        RefPhone = RefPhone;
                        TempData["error"] = Result.Message;
                        ModelState.AddModelError(string.Empty, Result.Message);

                        return Page();
                    }
                    return RedirectToPage("./Comfirmation", new { xcode = "whatsapp_api_data", xmal = Result.Mxemail, txtd = Result.Id });

                    //return RedirectToPage("./Verify", new { xmal = Result.Mxemail, txtd = Result.Id });
                }
                catch (Exception exception)
                {
                    Input.Password = null;
                    Input.ConfirmPassword = null;
                    await _exceptionLogger.LogAsync(exception, Input);
                    //return StatusCode(StatusCodes.Status500InternalServerError);
                    TempData["error"] = "unable to create account";
                    ModelState.AddModelError(string.Empty, "unable to create account");

                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }









    }
}
