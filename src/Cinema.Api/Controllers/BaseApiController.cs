using Microsoft.AspNetCore.Mvc;

namespace Cinema.Api.Controllers;

public abstract class BaseApiController : ControllerBase
{
    protected Guid UserId
    {
        get
        {
            var claimValue = User.FindFirst("id")?.Value;

            if (string.IsNullOrEmpty(claimValue)) throw new UnauthorizedAccessException("User ID claim is missing.");
            return Guid.Parse(claimValue);
        }
    }
}
