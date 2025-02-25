﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using JurayKV.Blazor.Common;
using JurayKV.Blazor.Models;
using JurayKV.Blazor.Models.IdentityModels;
using Microsoft.AspNetCore.WebUtilities;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace JurayKV.Blazor.Services;

[ScopedService]
public class UserService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public UserService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage)
    {
        httpClientFactory.ThrowIfNull(nameof(httpClientFactory));

        _httpClient = httpClientFactory.CreateClient("EmployeeManagementApi");
        _localStorage = localStorage;
    }

    public async Task<PaginatedList<UserModel>> GetListAsync(int pageIndex, int pageSize, UserSearchModel searchModel)
    {
        Dictionary<string, string> queryStrings = new Dictionary<string, string>()
        {
            ["pageIndex"] = pageIndex <= 0 ? "1" : pageIndex.ToString(CultureInfo.InvariantCulture),
            ["pageSize"] = pageSize <= 0 ? "10" : pageSize.ToString(CultureInfo.InvariantCulture),
        };

        if (searchModel != null)
        {
            if (!string.IsNullOrWhiteSpace(searchModel.FullName))
            {
                queryStrings["fullName"] = searchModel.FullName;
            }

            if (!string.IsNullOrWhiteSpace(searchModel.UserName))
            {
                queryStrings["userName"] = searchModel.UserName;
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Email))
            {
                queryStrings["email"] = searchModel.Email;
            }

            if (!string.IsNullOrWhiteSpace(searchModel.IsActive))
            {
                queryStrings["status"] = searchModel.IsActive;
            }
        }

        string uriWithQueryStrings = QueryHelpers.AddQueryString("users", queryStrings);

        PaginatedList<UserModel> lists = await _httpClient.GetFromJsonAsync<PaginatedList<UserModel>>(uriWithQueryStrings);
        return lists;
    }

    public async Task<UserDetailsModel> GetDetailsByIdAsync(int userId)
    {
        if (userId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(userId));
        }

        UserDetailsModel userDetailsModel = await _httpClient.GetFromJsonAsync<UserDetailsModel>($"users/{userId}");

        return userDetailsModel;
    }

    public async Task<HttpResponseMessage> UpdateAsync(EditUserModel editUserModel)
    {
        if (editUserModel == null)
        {
            throw new ArgumentNullException(nameof(editUserModel));
        }

        HttpResponseMessage httpResponseMessage = await _httpClient.PutAsJsonAsync($"users/{editUserModel.Id}", editUserModel);

        return httpResponseMessage;
    }

    public async Task<HttpResponseMessage> SetPasswordAsync(SetUserPasswordModel setUserPasswordModel)
    {
        if (setUserPasswordModel == null)
        {
            throw new ArgumentNullException(nameof(setUserPasswordModel));
        }

        HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync($"users/{setUserPasswordModel.UserId}/set-password", setUserPasswordModel);

        return httpResponseMessage;
    }

    public async Task<HttpResponseMessage> ChangePasswordAsync(ChangePasswordModel changePasswordModel)
    {
        if (changePasswordModel == null)
        {
            throw new ArgumentNullException(nameof(changePasswordModel));
        }

        HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync($"users/{changePasswordModel.UserId}/change-password", changePasswordModel);

        return httpResponseMessage;
    }

    public async Task<HttpResponseMessage> RegisterAsync(RegistrationModel registerModel)
    {
        if (registerModel == null)
        {
            throw new ArgumentNullException(nameof(registerModel));
        }

        HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync("v1/user/registration", registerModel);

        return httpResponseMessage;
    }

    public async Task<HttpResponseMessage> LoginAsync(LoginModel loginModel)
    {
        if (loginModel == null)
        {
            throw new ArgumentNullException(nameof(loginModel));
        }

        HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync("v1/user/login", loginModel);

        return httpResponseMessage;
    }

    private async Task<string> GetAccessTokenAsync()
    {
        LoggedInUserInfo loggedInUserInfo = await _localStorage.GetItemAsync<LoggedInUserInfo>(LocalStorageKey.Jwt);
        return loggedInUserInfo?.AccessToken;
    }
}
