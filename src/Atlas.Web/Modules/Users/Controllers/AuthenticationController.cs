﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Atlas.Users.Application.CQRS.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Http;
using Atlas.Shared.Presentation;
using Atlas.Shared.Domain.Results;
using Atlas.Users.Application.CQRS.Authentication.Commands.SignIn;
using Atlas.Users.Application.CQRS.Authentication.Commands.SignInWithToken;
using Atlas.Users.Application.CQRS.Authentication.Commands.SignOut;
using Atlas.Shared.Application.Abstractions;
using Atlas.Users.Module;

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
    public async Task<IActionResult> SignInUser([FromBody] SignInCommand command, CancellationToken cancellationToken)
    {
        await usersModule.SendCommand(command, cancellationToken);

        return Ok(Result.Success());
    }

    [HttpPost("sign-in-with-token")]
    public async Task<IActionResult> SignInUserWithToken([FromBody] SignInWithTokenCommand command, CancellationToken cancellationToken)
    {
        await usersModule.SendCommand(command, cancellationToken);
        return Ok(Result.Success());
    }

    [Authorize]
    [HttpPost("sign-out")]
    public async Task<IActionResult> SignOutUser(CancellationToken cancellationToken)
    {
        await usersModule.SendCommand(new SignOutCommand(), cancellationToken);
        return Ok(Result.Success());
    }
}
