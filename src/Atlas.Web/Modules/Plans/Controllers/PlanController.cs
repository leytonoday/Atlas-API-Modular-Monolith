using Atlas.Plans.Application.CQRS.Plans.Commands.AddFeatureToPlan;
using Atlas.Plans.Application.CQRS.Plans.Commands.CreatePlan;
using Atlas.Plans.Application.CQRS.Plans.Commands.RemoveFeature;
using Atlas.Plans.Application.CQRS.Plans.Commands.UpdateFeatureOnPlan;
using Atlas.Plans.Application.CQRS.Plans.Queries.GetAllPlans;
using Atlas.Plans.Application.CQRS.Plans.Queries.GetPlanById;
using Atlas.Plans.Application.CQRS.Plans.Queries.GetPlanByUserId;
using Atlas.Plans.Application.CQRS.Plans.Queries.GetPlanFeaturesByPlanId;
using Atlas.Plans.Module;
using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Domain.Results;
using Atlas.Web.Modules.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Atlas.Web.Modules.Plans.Controllers;

[Route("/api/{version:apiVersion}/plan")]
[ApiVersion("1.0")]
public class PlanController(IPlansModule plansModule, IExecutionContextAccessor executionContext) : ApiController
{
    [HttpGet("{planId:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid planId, CancellationToken cancellationToken)
    {
        var result = await plansModule.SendQuery(new GetPlanByIdQuery(planId), cancellationToken);
        return Ok(Result.Success(result));
    }

    [Authorize]
    [HttpGet("my-plan")]
    public async Task<IActionResult> GetMyPlan(CancellationToken cancellationToken)
    {
        var result = await plansModule.SendQuery(new GetPlanByUserIdQuery(executionContext.UserId), cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeInactive, CancellationToken cancellationToken)
    {
        var result = await plansModule.SendQuery(new GetAllPlansQuery(includeInactive), cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlan([FromBody] CreatePlanCommand createPlanCommand, CancellationToken cancellationToken)
    {
        var result = await plansModule.SendCommand(createPlanCommand, cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePlan([FromBody] UpdatePlanCommand updatePlanCommand, CancellationToken cancellationToken)
    {
        await plansModule.SendCommand(updatePlanCommand, cancellationToken);
        return Ok(Result.Success());
    }

    [HttpPost("features")]
    public async Task<IActionResult> AddFeatureToPlan([FromBody] AddFeatureToPlanCommand addFeatureToPlanCommand, CancellationToken cancellationToken)
    {
        await plansModule.SendCommand(addFeatureToPlanCommand, cancellationToken);
        return Ok(Result.Success());
    }

    [HttpDelete("features")]
    public async Task<IActionResult> RemoveFeatureFromPlan([FromBody] RemoveFeatureFromPlanCommand removeFeatureFromPlanCommand, CancellationToken cancellationToken)
    {
        await plansModule.SendCommand(removeFeatureFromPlanCommand, cancellationToken);
        return Ok(Result.Success());
    }

    [HttpPut("features")]
    public async Task<IActionResult> UpdateFeatureOnPlan([FromBody] UpdateFeatureOnPlanCommand updateFeatureOnPlanCommand, CancellationToken cancellationToken)
    {
        await plansModule.SendCommand(updateFeatureOnPlanCommand, cancellationToken);
        return Ok(Result.Success());
    }

    [HttpGet("{planId:guid}/features")]
    public async Task<IActionResult> GetPlanFeatures([FromRoute] Guid planId, CancellationToken cancellationToken)
    {
        var result = await plansModule.SendQuery(new GetPlanFeaturesByPlanIdQuery(planId), cancellationToken);
        return Ok(Result.Success(result));
    }
}

