using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JurayKV.Api.Endpoints.UserEndpoints;

[Authorize]
[Route("api/v{version:apiVersion}/user")]
[ApiController]
[ApiExplorerSettings(GroupName = "User Endpoints")]
public abstract class UserEndpointBase : ControllerBase
{
}
