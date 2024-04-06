using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Shared.Application.Abstractions.Messaging.Queue;
using Atlas.Tests.Shared;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Routing;
using NetArchTest.Rules;
using System.Reflection;

namespace Atlas.ArchTests.Shared;

public abstract class BaseApplicationTests(Assembly applicationAssembly)
{

    /// <summary>
    /// Ensures that command handlers are named correctly and have "CommandHandler" at the end. 
    /// </summary>
    [Fact]
    public void CommandHandler_Should_Have_Name_EndingWith_CommandHandler()
    {
        var result = Types.InAssembly(applicationAssembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
                .Or()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .And()
            .DoNotHaveNameMatching(".*Decorator.*").Should()
            .HaveNameEndingWith("CommandHandler")
            .GetResult();

        TestUtils.AssertSuccess(result);
    }

    /// <summary>
    /// Ensures that query handlers are named correctly and have "QueryHandler" at the end. 
    /// </summary>
    [Fact]
    public void QueryHandler_Should_Have_Name_EndingWith_QueryHandler()
    {
        var result = Types.InAssembly(applicationAssembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .Should()
            .HaveNameEndingWith("QueryHandler")
            .GetResult();

        TestUtils.AssertSuccess(result);
    }

    /// <summary>
    /// Ensures taht query and command handlers are internal. Only the command or query itself should be public (and therfore externally accessible), not the handler.
    /// </summary>
    [Fact]
    public void Command_And_Query_Handlers_Should_Not_Be_Public()
    {
        var types = Types.InAssembly(applicationAssembly)
            .That()
                .ImplementInterface(typeof(IQueryHandler<,>))
                    .Or()
                .ImplementInterface(typeof(ICommandHandler<>))
                    .Or()
                .ImplementInterface(typeof(ICommandHandler<,>))
                    .Or()
                .ImplementInterface(typeof(IQueuedCommandHandler<>))
            .Should().NotBePublic().GetResult().FailingTypes;

        TestUtils.AssertFailingTypes(types);
    }

    /// <summary>
    /// Ensures that validators are named correctly and have "Validator" at the end.
    /// </summary>
    [Fact]
    public void Validator_Should_Have_Name_EndingWith_Validator()
    {
        var result = Types.InAssembly(applicationAssembly)
            .That()
            .Inherit(typeof(AbstractValidator<>))
            .Should()
            .HaveNameEndingWith("Validator")
            .GetResult();

        TestUtils.AssertSuccess(result);
    }

    /// <summary>
    /// Ensures that MediatR handlers don't directly implement IRequest. We never want this to occur. They should directly implement some derived type that categorises them, 
    /// such as ICommandHandler or IQueryHandler.
    /// </summary>
    [Fact]
    public void MediatR_RequestHandler_Should_NotBe_Used_Directly()
    {
        var requestHandlerTypes = Types.InAssembly(applicationAssembly)
            .That().DoNotHaveName("ICommandHandler`1")
            .Should().ImplementInterface(typeof(IRequestHandler<>))
            .GetTypes();

        List<Type> failingTypes = [];
        foreach (var type in requestHandlerTypes)
        {
            bool isCommandHandler = type.GetInterfaces().Any(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
            bool isQueuedCommandHandler = type.GetInterfaces().Any(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(IQueuedCommandHandler<>));
            bool isCommandWithResultHandler = type.GetInterfaces().Any(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));
            bool isQueryHandler = type.GetInterfaces().Any(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition() == typeof(IQueryHandler<,>));
            if (!isCommandHandler && !isCommandWithResultHandler && !isQueryHandler && !isQueuedCommandHandler)
            {
                failingTypes.Add(type);
            }
        }

        TestUtils.AssertFailingTypes(failingTypes);
    }
}
