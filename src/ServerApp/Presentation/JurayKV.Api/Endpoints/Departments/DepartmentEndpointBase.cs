using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JurayKV.Api.Endpoints.Departments;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/departments")]
[ApiController]
[Authorize]
[ApiExplorerSettings(GroupName = "Department Endpoints")]
public abstract class DepartmentEndpointBase : ControllerBase
{
}
