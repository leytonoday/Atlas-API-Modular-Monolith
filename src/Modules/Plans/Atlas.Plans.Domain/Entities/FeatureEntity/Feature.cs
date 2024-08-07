﻿using Atlas.Plans.Domain.Entities.FeatureEntity.BusinessRules;
using Atlas.Plans.Domain.Entities.FeatureEntity.DomainEvents;
using Atlas.Plans.Domain.Entities.PlanEntity;
using Atlas.Shared.Domain.AggregateRoot;
using Atlas.Shared.Domain.Entities;

namespace Atlas.Plans.Domain.Entities.FeatureEntity;

public class Feature : Entity<Guid>, IAggregateRoot
{
    private Feature() { }

    /// <summary>
    /// Gets or sets the name of the <see cref="Feature"/>.
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    /// A unique string used to identify this feature.
    /// </summary>
    public string Code { get; private set; } = null!;

    /// <summary>
    /// Gets or sets whether the <see cref="Feature"/> is inheritable. If true, it means that when a <see cref="PlanFeatureEntity"/> has this as a feature, 
    /// other <see cref="PlanFeatureEntity"/> entities can inherit the feature.
    /// </summary>
    /// <remarks>An example of a feature that shouldn't be inheritable is one where there is a value on the <see cref="PlanFeature"/> join table is not <see langword="null"/>.
    public bool IsInheritable { get; private set; }

    /// <summary>
    /// Gets or sets the description of the <see cref="Feature"/>.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets or sets whether the <see cref="Feature"/> is hidden to users. Some features may exist on a <see cref="PlanFeatureEntity"/> must we don't want the user to know about it.
    /// </summary>
    public bool IsHidden { get; private set; }

    /// <summary>
    /// Gets or sets the <see cref="Plan"/> entities that have this <see cref="Feature"/>.
    /// </summary>
    public virtual ICollection<Plan>? Plans { get; private set; } = null;

    public static async Task<Feature> CreateAsync(
        string name,
        string description,
        bool isInheritable,
        bool isHidden,
        IFeatureRepository featureRepository,
        CancellationToken cancellationToken)
    {
        // Ensure the name is unique
        await CheckAsyncBusinessRule(new FeatureNameMustBeUniqueBusinessRule(name, featureRepository), cancellationToken);

        return new Feature { Name = name, Description = description, IsInheritable = isInheritable, IsHidden = isHidden, Code = NormaliseFeatureName(name) };
    }

    public static async Task<Feature> UpdateAsync(
        Feature feature,
        string name,
        string description,
        bool isInheritable,
        bool isHidden,
        IFeatureRepository featureRepository,
        CancellationToken cancellationToken)
    {
        bool hasIsInheritableChanged = isInheritable != feature.IsInheritable;

        // If the name has changed, ensure another Feature doesn't already have that name
        if (name != feature.Name)
        {
            await CheckAsyncBusinessRule(new FeatureNameMustBeUniqueBusinessRule(name, featureRepository), cancellationToken);
        }

        feature.Name = name;
        feature.Description = description;
        feature.IsInheritable = isInheritable;
        feature.IsHidden = isHidden;
        feature.Code = NormaliseFeatureName(name);

        feature.AddDomainEvent(new FeatureUpdatedDomainEvent(feature.Id, hasIsInheritableChanged));

        return feature;
    }

    private static string NormaliseFeatureName(string featureName)
    {
        return featureName.Replace(' ', '_').ToUpperInvariant();
    }
}
