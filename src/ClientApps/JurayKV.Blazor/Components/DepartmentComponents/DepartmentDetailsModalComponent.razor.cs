﻿using System;
using System.Threading.Tasks;
using JurayKV.Blazor.Models.DepartmentModels;
using JurayKV.Blazor.Services;

namespace JurayKV.Blazor.Components.DepartmentComponents;

public partial class DepartmentDetailsModalComponent
{
    private readonly DepartmentService _departmentService;

    public DepartmentDetailsModalComponent(DepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    private string ModalClass { get; set; } = string.Empty;

    private bool ShowBackdrop { get; set; }

    private DepartmentDetailsModel DepartmentDetailsModel { get; set; }

    public async Task ShowAsync(Guid departmentId)
    {
        DepartmentDetailsModel = await _departmentService.GetByIdAsync(departmentId);
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
}
