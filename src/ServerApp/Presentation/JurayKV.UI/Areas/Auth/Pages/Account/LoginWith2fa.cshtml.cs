// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using JurayKV.Application.Commands.UserManagerCommands;
using JurayKV.Application;
using MediatR;

namespace JurayKV.UI.Areas.Auth.Pages.Account
{
    public class LoginWith2faModel : PageModel
    {
        private readonly SignInManager<JurayKV.Domain.Aggregates.IdentityAggregate.ApplicationUser> _signInManager;
        private readonly UserManager<JurayKV.Domain.Aggregates.IdentityAggregate.ApplicationUser> _userManager;
        private readonly ILogger<LoginWith2faModel> _logger;
        private readonly IMediator _mediator;

        public LoginWith2faModel(
            SignInManager<JurayKV.Domain.Aggregates.IdentityAggregate.ApplicationUser> signInManager,
            UserManager<JurayKV.Domain.Aggregates.IdentityAggregate.ApplicationUser> userManager,
            ILogger<LoginWith2faModel> logger,
            IMediator mediator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
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
        public bool RememberMe { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

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
            [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Authenticator code")]
            public string TwoFactorCode { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember this machine")]
            public bool RememberMachine { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            ReturnUrl = returnUrl;
            RememberMe = rememberMe;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, Input.RememberMachine);

            var userId = await _userManager.GetUserIdAsync(user);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
                _logger.LogInformation("User logged in.");
                LastLoginCommand lst = new LastLoginCommand(user.Id.ToString());
                await _mediator.Send(lst);
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains(Constants.SuperAdminPolicy))
                {
                    return RedirectToPage("/Dashboard/Index", new { area = "KvMain" });
                }
                else if (roles.Contains(Constants.ClientPolicy))
                {
                    return RedirectToPage("/Account/Index", new { area = "Client" });
                }
               
                else if (roles.Contains(Constants.ManagerPolicy) 
                    || roles.Contains(Constants.AdminPolicy)
                    || roles.Contains(Constants.CompanyPolicy)
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
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return RedirectToPage("./Lockout");
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return Page();
            }
        }
    }
}
