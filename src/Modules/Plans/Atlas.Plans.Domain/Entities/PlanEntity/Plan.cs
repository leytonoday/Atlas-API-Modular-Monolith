using Atlas.Plans.Domain.Entities.FeatureEntity;
using Atlas.Plans.Domain.Entities.PlanEntity.Events;
using Atlas.Plans.Domain.Entities.PlanFeatureEntity;
using Atlas.Plans.Domain.Errors;
using Atlas.Plans.Domain.Services;
using Atlas.Shared.Domain.AggregateRoot;
using Atlas.Shared.Domain.Exceptions;

namespace Atlas.Plans.Domain.Entities.PlanEntity;

public class Plan : AggregateRoot<Guid>
{
    /// <summary>
    /// Gets or sets the name of this <see cref="Plan"/>.
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the description of this <see cref="Plan"/>.
    /// </summary>
    public string Description { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the Stripe Product Id associated with this <see cref="Plan"/>.
    /// </summary>
    /// <remarks>Nullable because we programmatically create the product in Stripe.</remarks>
    public string? StripeProductId { get; private set; } = null;

    /// <summary>
    /// Gets or sets the ISO 4217 currency code that represents the currency that the the price of this <see cref="Plan"/> is using.
    /// </summary>
    public string IsoCurrencyCode { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the monthly price of this <see cref="Plan"/>. This is the amount that users will be charged by Stripe every month.
    /// </summary>
    public int MonthlyPrice { get; private set; }

    /// <summary>
    /// Gets or sets the annual price of this <see cref="Plan"/>. This is the amount that users will be charged by Stripe every year.
    /// </summary>
    public int AnnualPrice { get; private set; }

    /// <summary>
    /// Gets or sets the amount of days that a user can use this <see cref="Plan"/> for free before they are charged.
    /// </summary>
    public int TrialPeriodDays { get; private set; }

    /// <summary>
    /// Gets or sets a tag that can be used to signify that this <see cref="Plan"/> is special in some way. 
    /// </summary>
    /// <remarks>An example tag would be "Popular!".</remarks>
    public string? Tag { get; private set; } = null;

    /// <summary>
    /// Gets or sets the naeme of the icon to display for this product. The name represents an object in the "react-icons" npm library.
    /// </summary>
    public string Icon { get; private set; } = null!;

    /// <summary>
    /// Gets or sets the colours of the icon. Uses antd primary if not specified.
    /// </summary>
    public string? IconColour { get; private set; } = null;

    /// <summary>
    /// Gets or sets the background colour to display for the plan card on the pricing page of the website. If not specified, a default colour will be used.
    /// </summary>
    /// <remarks>Use this to convey that a particular <see cref="Plan"/> is important. For example, an "Ultimate" or "Pro" <see cref="Plan"/>.</remarks>
    public string? BackgroundColour { get; private set; } = null;

    /// <summary>
    /// Gets or sets the text colour to display for the plan card on the pricing page of the website. If not specified, a default colour will be used.
    /// </summary>
    public string? TextColour { get; private set; } = null;

    /// <summary>
    /// Gets or sets whether this Plan is active or not. If not, it will be archived, and not displayed to customers on the website. The corresponding
    /// Stripe product is also archived.
    /// </summary>
    public bool Active { get; private set; }

    /// <summary>
    /// Gets or sets the Id of the <see cref="Plan"/> that this <see cref="Plan"/> inherits <see cref="PlanFeature"/>s from.
    /// </summary>
    public Guid? InheritsFromId { get; private set; }

    /// <summary>
    /// Gets or sets the <see cref="Feature"/>s of this <see cref="Plan"/>.
    /// </summary>
    public virtual ICollection<Feature>? Features { get; private set; } = null;

    /// <summary>
    /// Gets or sets the <see cref="PlanFeature"/>s of this <see cref="Plan"/>.
    /// </summary>
    public virtual ICollection<PlanFeature>? PlanFeatures { get; private set; } = null;

    public static async Task<Plan> CreateAsync(
        string name,
        string description,
        string isoCurrencyCode,
        int monthyPrice,
        int annualPrice,
        int trialPeriodDays,
        string? tag,
        string icon,
        string iconColour,
        string? backgroundColour,
        string? textColour,
        bool active,
        Guid? inheritsFromId,
        PlanService planService,
        CancellationToken cancellationToken
        )
    {
        bool isNameTaken = await planService.IsNameTakenAsync(name, cancellationToken);
        if (isNameTaken)
        {
            throw new ErrorException(PlansDomainErrors.Plan.NameMustBeUnique);
        }

        var plan = new Plan()
        {
            Name = name,
            Description = description,
            IsoCurrencyCode = isoCurrencyCode,
            MonthlyPrice = monthyPrice,
            AnnualPrice = annualPrice,
            TrialPeriodDays = trialPeriodDays,
            Tag = tag,
            Icon = icon,
            IconColour = iconColour,
            BackgroundColour = backgroundColour,
            TextColour = textColour,
            Active = active,
            InheritsFromId = inheritsFromId
        };

        plan.AddDomainEvent(new PlanCreatedEvent(Guid.NewGuid(), name));

        return plan;
    }

    public static async Task<Plan> UpdateAsync(
        Guid id,
        string name,
        string description,
        string isoCurrencyCode,
        int monthyPrice,
        int annualPrice,
        int trialPeriodDays,
        string? tag,
        string icon,
        string iconColour,
        string? backgroundColour,
        string? textColour,
        bool active,
        Guid? inheritsFromId,
        PlanService planService,
        IStripeService stripeService,
        IPlanRepository planRepository,
        CancellationToken cancellationToken)
    {
        Plan plan = await planRepository.GetByIdAsync(id, true, cancellationToken)
            ?? throw new ErrorException(PlansDomainErrors.Plan.PlanNotFound);

        // If the name has been changed, ensure that a Plan with this name doesn't already exist
        if (name.ToLower() != plan.Name.ToLower() && await planService.IsNameTakenAsync(name, cancellationToken))
        {
            throw new ErrorException(PlansDomainErrors.Plan.NameMustBeUnique);
        }

        bool hasBeenReactivated = !plan.Active && active; // Has the plan been re-activated?
        bool hasBeenDeactivated = plan.Active && !active; // Has the plan de-activated?
        bool havePricesChanged = plan.MonthlyPrice != monthyPrice || plan.AnnualPrice != annualPrice;

        if (hasBeenDeactivated && await stripeService.DoesPlanHaveActiveSubscriptions(plan, cancellationToken))
        {
            throw new ErrorException(PlansDomainErrors.Plan.CannotDeactivateWithActiveSubscribers);
        }

        // Update fields manually
        plan.Name = name;
        plan.Description = description;
        plan.IsoCurrencyCode = isoCurrencyCode;
        plan.MonthlyPrice = monthyPrice;
        plan.AnnualPrice = annualPrice;
        plan.TrialPeriodDays = trialPeriodDays;
        plan.Tag = tag;
        plan.Icon = icon;
        plan.IconColour = iconColour;
        plan.BackgroundColour = backgroundColour;
        plan.TextColour = textColour;
        plan.Active = active;
        plan.InheritsFromId = inheritsFromId;

        // Check for circular dependencies
        if (plan.InheritsFromId.HasValue && await planService.IsCircularInheritanceDetectedAsync(plan, cancellationToken))
        {
            throw new ErrorException(PlansDomainErrors.Plan.CircularInheritanceDetected);
        }

        // If the plan has been de-activated, make sure that no other plans inherit from this plan.
        if (hasBeenDeactivated)
        {
            Plan? targetPlan = await planRepository.GetByInheritsFromIdAsync(plan.Id, true, cancellationToken);
            if (targetPlan is not null)
                targetPlan.InheritsFromId = null;
        }

        plan.AddDomainEvent(new PlanUpdatedEvent(Guid.NewGuid(), plan.Id, hasBeenDeactivated, hasBeenReactivated, havePricesChanged));

        return plan;
    }

    /// <summary>
    /// Creates several entities within Stripe that correspond to this Plan, such as a price based on the monthly and annual proces, and a product Id.
    /// </summary>
    /// <param name="plan">The plan to create the Stripe entities for.</param>
    /// <param name="stripeService">Stores various methods for interactive with the Stripe API.</param>
    /// <param name="cancellationToken">Propogates notification that operations should be cancelled.</param>
    /// <returns>A <see cref="Task"/> that returns when operations are complete.</returns>
    public static async Task CreateStripeEntities(Plan plan, IStripeService stripeService, CancellationToken cancellationToken)
    {
        string strpeProductId = await stripeService.CreateStripeProductAsync(plan, cancellationToken);
        plan.StripeProductId = strpeProductId;

        await stripeService.CreatePricesForPlanAsync(plan, cancellationToken);
    }
}
