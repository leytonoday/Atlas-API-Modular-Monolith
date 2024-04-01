using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Atlas.Users.Application.CQRS.Users.Queries.GetUsersByPlanId;
using Atlas.Users.Application.CQRS.Users.Queries.GetRolesByUserId;
using Atlas.Users.Application.CQRS.Users.Commands.CreateUser;
using Atlas.Shared.Domain.Results;
using Atlas.Users.Application.CQRS.Users.Commands.UpdateUser;
using Atlas.Users.Application.CQRS.Users.Commands.DeleteUser;
using Atlas.Users.Application.CQRS.Users.Commands.ConfirmUserEmail;
using Atlas.Users.Application.CQRS.Users.Commands.RefreshConfirmUserEmail;
using Atlas.Users.Application.CQRS.Users.Commands.ForgotPassword;
using Atlas.Users.Application.CQRS.Users.Commands.ResetPassword;
using Atlas.Users.Application.CQRS.Users.Commands.ChangePassword;
using Atlas.Shared.Application.Abstractions;
using Atlas.Users.Module;
using Atlas.Web.Modules.Shared;

namespace Atlas.Web.Modules.Users.Controllers;

[Route("/api/{version:apiVersion}/user")]
[ApiVersion("1.0")]
public class UserController(IExecutionContextAccessor executionContext, IUsersModule usersModule) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var result = await usersModule.SendCommand(command, cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        await usersModule.SendCommand(command, cancellationToken);
        return Ok(Result.Success());
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteUser([FromBody] DeleteUserCommand command, CancellationToken cancellationToken)
    {
        await usersModule.SendCommand(command, cancellationToken);
        return Ok(Result.Success());
    }

    [HttpGet("my-roles")]
    public async Task<IActionResult> GetMyRoles(CancellationToken cancellationToken)
    {
        var result = await usersModule.SendQuery(new GetRolesByUserIdQuery(executionContext.UserId), cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmUserEmail([FromBody] ConfirmUserEmailCommand command, CancellationToken cancellationToken)
    {
        var result = await usersModule.SendCommand(command, cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpPost("refresh-confirm-email/{identifier}")]
    public async Task<IActionResult> RefreshConfirmUserEmail([FromRoute] string identifier, CancellationToken cancellationToken)
    {
        await usersModule.SendCommand(new RefreshConfirmUserEmailCommand(identifier), cancellationToken);
        return Ok(Result.Success());
    }

    [HttpPost("forgot-password/{identifier}")]
    public async Task<IActionResult> ForgotPassword([FromRoute] string identifier, CancellationToken cancellationToken)
    {
        await usersModule.SendCommand(new ForgotPasswordCommand(identifier), cancellationToken);
        return Ok(Result.Success());
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        await usersModule.SendCommand(command, cancellationToken);
        return Ok(Result.Success());
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        await usersModule.SendCommand(command, cancellationToken);
        return Ok(Result.Success());
    }

    [HttpGet("plan/{planId}")]
    public async Task<IActionResult> GetAllUsersOnPlan([FromRoute] Guid planId, CancellationToken cancellationToken)
    {
        var result = await usersModule.SendQuery(new GetUsersByPlanIdQuery(planId), cancellationToken);
        return Ok(Result.Success(result));
    }
}

