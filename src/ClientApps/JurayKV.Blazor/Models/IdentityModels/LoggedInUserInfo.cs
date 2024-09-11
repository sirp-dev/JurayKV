﻿using System;

namespace JurayKV.Blazor.Models.IdentityModels;

public class LoggedInUserInfo
{
    public Guid UserId { get; set; }

    public string FullName { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string AccessToken { get; set; }

    public DateTime AccessTokenExpireAtUtc { get; set; }

    public string RefreshToken { get; set; }

    public DateTime RefreshTokenExpireAtUtc { get; set; }
}
