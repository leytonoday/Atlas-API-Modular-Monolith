using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Atlas.Users.Application.CQRS.Users.Queries.GetUserById;
using Atlas.Shared.Domain.Results;
using Atlas.Shared.Application.Abstractions;
using Atlas.Web.Modules.Shared;
using Atlas.Users.Application.CQRS.Authentication.Queries.CanSignIn;
using Atlas.Users.Application.CQRS.Authentication.Queries.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Atlas.Users.Application.CQRS.Authentication.Queries.CanSignInWithToken;
using Atlas.Users.Infrastructure.Module;

namespace Atlas.Web.Modules.Users.Controllers;

[Route("/api/{version:apiVersion}/authentication")]
[ApiVersion("1.0")]
public class AuthenticationController(IExecutionContextAccessor executionContext, IUsersModule usersModule) : ApiController
{
    [HttpGet("is-authenticated")]
    public IActionResult IsAuthenticated()
    {
        bool result = HttpContext.User.Identity?.IsAuthenticated ?? false;
        return Ok(Result.Success(result));
    }

    [Authorize]
    [HttpGet("who-am-i")]
    public async Task<IActionResult> WhoAmI(CancellationToken cancellationToken)
    {
        var result = await usersModule.SendQuery(new GetUserByIdQuery(executionContext.UserId), cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignInUser([FromBody] CanSignInQuery query, CancellationToken cancellationToken)
    {
        CanSignInResponse result = await usersModule.SendQuery(query, cancellationToken);
        if (result.IsSuccess)
        {
            await SignInAsync(result.UserId, result.UserName, result.Email, result.Roles);
        }

        return Ok(Result.Success());
    }

    [HttpPost("sign-in-with-token")]
    public async Task<IActionResult> SignInUserWithToken([FromBody] CanSignInWithTokenQuery query, CancellationToken cancellationToken)
    {
        CanSignInResponse result = await usersModule.SendQuery(query, cancellationToken);
        if (result.IsSuccess)
        {
            await SignInAsync(result.UserId, result.UserName, result.Email, result.Roles);
        }

        return Ok(Result.Success());
    }

    [Authorize]
    [HttpPost("sign-out")]
    public async Task<IActionResult> SignOutUser()
    {
        await SignOutAsync();
        return Ok(Result.Success());
    }

    private async Task SignInAsync(Guid userId, string userName, string email, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, userId.ToString()),
            new (ClaimTypes.Email, email),
            new ("UserName", userName),
        };

        // Add all roles to the user's claims
        foreach(string role in roles)
        {
            claims.Add(new(ClaimTypes.Role, role));
        }

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            // Refreshing the authentication session should be allowed.

            IsPersistent = true,
            // Whether the authentication session is persisted across 
            // multiple requests. When used with cookies, controls
            // whether the cookie's lifetime is absolute (matching the
            // lifetime of the authentication ticket) or session-based.
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }

    private async Task SignOutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}

