using Cinema.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Cinema.Api.Attributes;

public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params Role[] roles)
    {
        Roles = string.Join(",", roles.Select(r => r.ToString()));
    }
}
