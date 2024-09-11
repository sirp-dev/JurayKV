﻿using JurayKV.Api.Endpoints.Employees;
using JurayKV.Application.Commands.DepartmentCommands;
using JurayKV.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace JurayKV.Api.Endpoints.Departments;

 public class DeleteDepartmentEndpoint : DepartmentEndpointBase
{
    private readonly IMediator _mediator;

    public DeleteDepartmentEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpDelete("{departmentId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    [SwaggerOperation(Summary = "Delete an existing department by department id.")]
    public async Task<IActionResult> Delete(Guid departmentId)
    {
        try
        {
            if (departmentId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, $"The value of {nameof(departmentId)} must be not empty.");
                return ValidationProblem(ModelState);
            }

            DeleteDepartmentCommand command = new DeleteDepartmentCommand(departmentId);
            await _mediator.Send(command);

            return NoContent();
        }
        catch (Exception exception)
        {
            if (exception is EntityNotFoundException)
            {
                ModelState.AddModelError(nameof(departmentId), "The Department does not exist.");
                return ValidationProblem(ModelState);
            }

            throw;
        }
    }
}
