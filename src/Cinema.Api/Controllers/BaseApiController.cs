using Microsoft.AspNetCore.Mvc;

namespace Cinema.Api.Controllers;

public abstract class BaseApiController : ControllerBase
{
    protected Guid UserId => Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
}
