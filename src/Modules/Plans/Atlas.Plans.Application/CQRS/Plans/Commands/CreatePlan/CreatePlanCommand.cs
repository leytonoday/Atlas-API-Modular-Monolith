using Atlas.Plans.Application.CQRS.Plans.Shared;
using MediatR;

namespace Atlas.Plans.Application.CQRS.Plans.Commands.CreatePlan;

public class CreatePlanCommand : BasePlanDto, IRequest<Guid>;
