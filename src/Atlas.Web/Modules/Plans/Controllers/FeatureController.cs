using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Atlas.Shared;
using Atlas.Plans.Infrastructure.CQRS.Features.Queries.GetAllFeatures;
using Atlas.Shared.Domain.Results;
using Atlas.Plans.Application.CQRS.Features.Commands.CreateFeature;
using Atlas.Plans.Application.CQRS.Features.Commands.UpdateFeature;
using Atlas.Plans.Application.CQRS.Features.Commands.DeleteFeature;
using Atlas.Web.Modules.Shared;
using Atlas.Plans.Infrastructure.Module;

namespace Atlas.Web.Modules.Plans.Controllers;

[Authorize(Roles = RoleNames.Administrator)]
[Route("/api/{version:apiVersion}/feature")]
[ApiVersion("1.0")]
public class FeatureController(IPlansModule plansModule) : ApiController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllFeatures(CancellationToken cancellationToken)
    {
        var result = await plansModule.SendQuery(new GetAllFeaturesQuery(), cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpPost]
    public async Task<IActionResult> CreateFeature([FromBody] CreateFeatureCommand createFeatureCommand, CancellationToken cancellationToken)
    {
        var result = await plansModule.SendCommand(createFeatureCommand, cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateFeature([FromBody] UpdateFeatureCommand updateFeatureCommand, CancellationToken cancellationToken)
    {
        var result = await plansModule.SendCommand(updateFeatureCommand, cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpDelete("{featureId:guid}")]
    public async Task<IActionResult> DeleteFeature([FromRoute] Guid featureId, CancellationToken cancellationToken)
    {
        var result = await plansModule.SendCommand(new DeleteFeatureCommand(featureId), cancellationToken);
        return Ok(Result.Success(result));
    }
}

