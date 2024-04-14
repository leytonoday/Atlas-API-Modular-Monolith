using Atlas.Plans.Application.CQRS.CreditTrackers.Queries.GetUserCreditTracker;
using Atlas.Plans.Infrastructure.Module;
using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Domain.Results;
using Atlas.Web.Modules.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atlas.Web.Modules.Plans.Controllers;

[Route("/api/{version:apiVersion}/credit-tracker")]
[ApiVersion("1.0")]
public class CreditTrackerController(IPlansModule plansModule, IExecutionContextAccessor executionContext) : ApiController
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserCreditTracker(CancellationToken cancellationToken)
    {
        var result = await plansModule.SendQuery(new GetUserCreditTrackerQuery(executionContext.UserId), cancellationToken);
        return Ok(Result.Success(result));
    }
}

