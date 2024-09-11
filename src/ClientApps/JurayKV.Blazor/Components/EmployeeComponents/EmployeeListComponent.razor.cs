﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JurayKV.Blazor.Common;
using JurayKV.Blazor.Models.EmployeeModels;
using JurayKV.Blazor.Services;

namespace JurayKV.Blazor.Components.EmployeeComponents;

public partial class EmployeeListComponent
{
    private readonly EmployeeService _employeeService;
    private readonly ExceptionLogger _exceptionLogger;

    public EmployeeListComponent(EmployeeService employeeService, ExceptionLogger exceptionLogger)
    {
        _employeeService = employeeService;
        _exceptionLogger = exceptionLogger;
    }

    private List<EmployeeDetailsModel> Employees { get; set; }

    private CreateEmployeeModalComponent CreateModal { get; set; }

    private UpdateEmployeeModalComponent UpdateModal { get; set; }

    private EmployeeDetailsModalComponent DetailsModal { get; set; }

    private DeleteEmployeeModalComponent DeleteModal { get; set; }

    private string ErrorMessage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Employees = await _employeeService.GetListAsync();
        }
        catch (HttpRequestException httpException)
        {
            Console.WriteLine(httpException);
            ErrorMessage = "The server maybe down or the app does not have CORS access to the requested server.";
        }
        catch (Exception exception)
        {
            await _exceptionLogger.LogAsync(exception);
            ErrorMessage = "There is some error.";
        }
    }

    private async Task OpenCreateModalAsync()
    {
        await CreateModal.OpenAsync();
    }

    private async Task OpenUpdateModalAsync(Guid employeeId)
    {
        await UpdateModal.OpenAsync(employeeId);
    }

    private async Task OpenDetailsModalAsync(Guid employeeId)
    {
        await DetailsModal.OpenAsync(employeeId);
    }

    private async Task OpenDeleteModalAsync(Guid employeeId)
    {
        await DeleteModal.OpenAsync(employeeId);
    }

    private async void CreatedEmployeeEventHandler()
    {
        Employees = await _employeeService.GetListAsync();
        StateHasChanged();
    }

    private async void EmployeeUpdatedEventHandler()
    {
        Employees = await _employeeService.GetListAsync();
        StateHasChanged();
    }

    private async void EmployeeDeletedEventHandler()
    {
        Employees = await _employeeService.GetListAsync();
        StateHasChanged();
    }
}
