﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JurayKV.Blazor.Common;
using JurayKV.Blazor.Models;
using JurayKV.Blazor.Models.EmployeeModels;
using JurayKV.Blazor.Services;
using Microsoft.AspNetCore.Components;
using TanvirArjel.Blazor.Components;
using TanvirArjel.Blazor.Utilities;

namespace JurayKV.Blazor.Components.EmployeeComponents;

public partial class CreateEmployeeModalComponent
{
    private readonly EmployeeService _employeeService;
    private readonly DepartmentService _departmentService;
    private readonly ExceptionLogger _exceptionLogger;

    public CreateEmployeeModalComponent(
        EmployeeService employeeService,
        DepartmentService departmentService,
        ExceptionLogger exceptionLogger)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
        _exceptionLogger = exceptionLogger;
    }

    [Parameter]
    public EventCallback EmployeeCreated { get; set; }

    private string ModalClass { get; set; } = string.Empty;

    private bool ShowBackdrop { get; set; }

    private CustomValidationMessages CustomValidationMessages { get; set; }

    private CreateEmployeeModel CreateEmployeeViewModel { get; set; } = new CreateEmployeeModel();

    private List<SelectListItem> DepartmentSelectList { get; set; } = new List<SelectListItem>();

    public async Task OpenAsync()
    {
        CreateEmployeeViewModel = new CreateEmployeeModel();
        List<SelectListItem> items = await _departmentService.GetSelectListAsync();
        DepartmentSelectList = items;

        ModalClass = "show d-block";
        ShowBackdrop = true;
        StateHasChanged();
    }

    private void Close()
    {
        ModalClass = string.Empty;
        ShowBackdrop = false;
        StateHasChanged();
    }

    private async Task HandleValidSubmit()
    {
        try
        {
            HttpResponseMessage httpResponseMessage = await _employeeService.CreateAsync(CreateEmployeeViewModel);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                Close();
                await EmployeeCreated.InvokeAsync();
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
