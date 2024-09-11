// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using JurayKV.Application;
using JurayKV.Application.Commands.UserManagerCommands;
using JurayKV.Application.Services;
using JurayKV.Domain.Aggregates.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace JurayKV.UI.Areas.Auth.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IMediator _mediator;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger, UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userManager = userManager;
            _mediator = mediator;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public string RefX { get; set; }
        public async Task OnGetAsync(string returnUrl = null, string refx = null)
        {
            RefX = refx;
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user != null)
                {
                    if (user.AccountStatus == Domain.Primitives.Enum.AccountStatus.Suspended)
                    {
                        return RedirectToPage("./Locked", new { id = user.Id });
                    }
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "You must confirm your email before logging in. Please check your email and enter the code.");
                        string errorMessage = ModelState.GetAllErrors();
                        string maskedEmail = EmailMask.MaskEmail(user.Email);

                        return RedirectToPage("./Verify", new { xmal = maskedEmail, txtd = user.Id });
                    }

                    //check password
                    var checkpass = await _userManager.CheckPasswordAsync(user, Input.Password);
                    if (checkpass == true)
                    {
                        if (user.TwoFactorEnabled == true)
                        {
                            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                            if (result.RequiresTwoFactor)
                            {
                                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                            }
                            if (result.IsLockedOut)
                            {
                                _logger.LogWarning("User account locked out.");
                                return RedirectToPage("./Lockout");
                            }
                            else
                            {


                                // Get all error messages
                                string allErrorMessages = string.Join(" | ", ModelState.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));

                                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                                return Page();
                            }
                        }
                        else
                        {
                            _logger.LogInformation("User logged in.");

                            await _signInManager.SignInAsync(user, null);

                            LastLoginCommand lst = new LastLoginCommand(user.Id.ToString());
                            await _mediator.Send(lst);
                            var roles = await _userManager.GetRolesAsync(user);

                            if (roles.Contains(Constants.SuperAdminPolicy))
                            {
                                return RedirectToPage("/Dashboard/Index", new { area = "KvMain" });
                            }
                            else if (roles.Contains(Constants.ClientPolicy) || roles.Contains(Constants.CompanyPolicy))
                            {
                                return RedirectToPage("/Account/Index", new { area = "Client" });
                            }

                            else if (roles.Contains(Constants.ManagerPolicy)
                                || roles.Contains(Constants.AdminPolicy)
                                
                                || roles.Contains(Constants.BucketPolicy)
                                || roles.Contains(Constants.ExchangeRatePolicy)
                                || roles.Contains(Constants.AdvertPolicy)
                                || roles.Contains(Constants.UsersManagerPolicy)
                                || roles.Contains(Constants.PointPolicy)
                                || roles.Contains(Constants.AdminOne)
                                || roles.Contains(Constants.AdminTwo)
                                || roles.Contains(Constants.AdminThree)
                                || roles.Contains(Constants.SliderPolicy)
                                || roles.Contains(Constants.ValidatorPolicy)
                                || roles.Contains(Constants.Transaction)
                                || roles.Contains(Constants.Permission)

                                )
                            {
                                return RedirectToPage("/Dashboard/Index", new { area = "KvMain" });
                            }

                            else if (roles.Contains(Constants.Dashboard))
                            {
                                return RedirectToPage("/Account/Index", new { area = "User" });
                            }
                            // Add more role-based redirections as needed

                            // If none of the role-based redirections match, you can have a default fallback
                            return RedirectToPage("/Index");
                        }
                    }

                    //if (!await _userManager.IsPhoneNumberConfirmedAsync(user))
                    //{
                    //    ModelState.AddModelError(string.Empty, "You must confirm your email before logging in. Please check your email and enter the code.");
                    //    string errorMessage = ModelState.GetAllErrors();
                    //    string maskedEmail = EmailMask.MaskEmail(user.Email);

                    //    return RedirectToPage("./VerifyPhone", new { xmal = maskedEmail, txtd = user.Id });
                    //}
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    //var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                    //if (result.RequiresTwoFactor)
                    //{
                    //    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    //}
                    //if (result.IsLockedOut)
                    //{
                    //    _logger.LogWarning("User account locked out.");
                    //    return RedirectToPage("./Lockout");
                    //}
                    else
                    {


                        // Get all error messages
                        string allErrorMessages = string.Join(" | ", ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage));

                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
