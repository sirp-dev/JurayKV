﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using JurayKV.Blazor.Common;
using JurayKV.Blazor.Models.DepartmentModels;
using JurayKV.Blazor.Services;
using Microsoft.AspNetCore.Components;
using TanvirArjel.ArgumentChecker;
using TanvirArjel.Blazor.Components;
using TanvirArjel.Blazor.Utilities;

namespace JurayKV.Blazor.Components.DepartmentComponents;

public partial class DeleteDepartmentModalComponent
{
    private readonly DepartmentService _departmentService;
    private readonly ExceptionLogger _exceptionLogger;

    public DeleteDepartmentModalComponent(DepartmentService departmentService, ExceptionLogger exceptionLogger)
    {
        _departmentService = departmentService;
        _exceptionLogger = exceptionLogger;
    }

    [Parameter]
    public EventCallback DepartmentDeleted { get; set; }

    private string ModalClass { get; set; } = string.Empty;

    private bool ShowBackdrop { get; set; }

    private DepartmentDetailsModel DepartmentDetailsModel { get; set; }

    private CustomValidationMessages CustomValidationMessages { get; set; }

    public async Task ShowAsync(Guid departmentId)
    {
        DepartmentDetailsModel = await _departmentService.GetByIdAsync(departmentId);

        ModalClass = "show d-block";
        ShowBackdrop = true;
        StateHasChanged();
    }

    protected override void OnInitialized()
    {
        DepartmentDetailsModel = new DepartmentDetailsModel();
    }

    private void Close()
    {
        ModalClass = string.Empty;
        ShowBackdrop = false;
        StateHasChanged();
    }

    private async Task HandleValidSubmit(Guid departmentId)
    {
        try
        {
            departmentId.ThrowIfEmpty(nameof(departmentId));

            HttpResponseMessage httpResponseMessage = await _departmentService.DeleteAsync(departmentId);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                Close();
                await DepartmentDeleted.InvokeAsync();
                return;
            }

            await CustomValidationMessages.AddAndDisplayAsync(httpResponseMessage);
        }
        catch (Exception exception)
        {
            CustomValidationMessages.AddAndDisplay(string.Empty, ErrorMessages.ClientErrorMessage);
            await _exceptionLogger.LogAsync(exception);
        }
    }
}
