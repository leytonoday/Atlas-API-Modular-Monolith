using Atlas.Shared.Domain.BusinessRules;
using Atlas.Shared.Domain.Entities;
using Atlas.Shared.Domain.Events;
using Atlas.Tests.Shared;
using NetArchTest.Rules;
using System.Reflection;

namespace Atlas.ArchTests.Shared;

/// <summary>
/// Various tests have been taken from the https://github.com/kgrzybek/modular-monolith-with-ddd repository
/// </summary>
public abstract class BaseDomainTests(Assembly domainAssembly)
{
    /// <summary>
    /// Ensures that all <see cref="Entity"/> have a parameterless private constructor, which is required in Domain-Driven-Design, to prevent uncontrolled external instantiation.
    /// </summary>
    [Fact]
    public void Entity_Should_Have_Parameterless_Private_Constructor()
    {

        var test = Types.InAssembly(domainAssembly).GetTypes().ToList();

        var entityTypes = Types.InAssembly(domainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .GetTypes().ToList();

        List<Type> failingTypes = [];
        foreach (var entityType in entityTypes)
        {
            bool hasPrivateParameterlessConstructor = false;
            var constructors = entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var constructorInfo in constructors)
            {
                if (constructorInfo.IsPrivate && constructorInfo.GetParameters().Length == 0)
                {
                    hasPrivateParameterlessConstructor = true;
                }
            }

            if (!hasPrivateParameterlessConstructor)
            {
                failingTypes.Add(entityType);
            }
        }

        TestUtils.AssertFailingTypes(failingTypes);
    }

    /// <summary>
    /// All <see cref="IDomainEvent"/> should have "DomainEvent" at the end of their name.
    /// </summary>
    [Fact]
    public void DomainEvent_Should_Have_DomainEventPostfix()
    {
        var result = Types.InAssembly(domainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should().HaveNameEndingWith("DomainEvent")
            .GetResult();

        TestUtils.AssertSuccess(result);
    }

    /// <summary>
    /// All <see cref="IBusinessRule"/> and <see cref="IAsyncBusinessRule"/> should have "BusinessRule" at the end of their name.
    /// </summary>
    [Fact]
    public void BusinessRule_Should_Have_RulePostfix()
    {
        var result = Types.InAssembly(domainAssembly)
            .That()
            .ImplementInterface(typeof(IBusinessRule))
            .Or()
            .ImplementInterface(typeof(IAsyncBusinessRule))
            .Should().HaveNameEndingWith("Rule")
            .GetResult();

        TestUtils.AssertSuccess(result);
    }
}
