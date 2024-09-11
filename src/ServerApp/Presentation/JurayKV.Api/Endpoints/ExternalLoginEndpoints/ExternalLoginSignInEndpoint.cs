﻿using JurayKV.Domain.Aggregates.IdentityAggregate;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JurayKV.Api.Endpoints.ExternalLoginEndpoints;

[ApiVersion("1.0")]
public class ExternalLoginSignInEndpoint : ExternalLoginEndpointBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public ExternalLoginSignInEndpoint(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpGet("sign-in")]
    public IActionResult Get(string provider)
    {
        // Request a redirect to the external login provider.
        string redirectUrl = "api/v1/external-login/sign-in-callback";
        AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }
}
