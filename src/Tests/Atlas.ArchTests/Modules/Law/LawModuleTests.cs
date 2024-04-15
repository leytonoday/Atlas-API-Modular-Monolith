using ArchTests.Shared;
using Atlas.ArchTests.Shared;
using Atlas.Users.Application;
using Atlas.Users.Infrastructure;
using System.Reflection;

namespace Atlas.ArchTests.Modules.Law;

public class LawModuleTests : BaseModuleTests
{
    /// <summary>
    /// <inheritdoc cref="BaseModuleTests.Module_DoesNotHave_Dependency_On_Other_Modules"/>
    /// </summary>
    [Fact]
    public void LawModule_DoesNotHave_Dependency_On_Other_Modules()
    {
        // Arrange
        List<string> otherModules = [Namespaces.PlansNamespace, Namespaces.UsersNamespace];
        List<Assembly> usersAssemblies = [
            typeof(UsersInfrastructureAssemblyReference).Assembly,
            typeof(UsersApplicationAssemblyReference).Assembly,
            typeof(UsersDomainAssemblyReference).Assembly,
        ];

        Module_DoesNotHave_Dependency_On_Other_Modules(otherModules, usersAssemblies);
    }

    /// <summary>
    /// <inheritdoc cref="BaseModuleTests.Domain_DoesNotDependOn_OuterLayers"/>
    /// </summary>
    [Fact]
    public void Law_Domain_DoesNotDependOn_OuterLayers()
    {
        Domain_DoesNotDependOn_OuterLayers(Namespaces.LawNamespace);
    }

    /// <summary>
    /// <inheritdoc cref="BaseModuleTests.Application_DoesNotDependOn_OuterLayers"/>
    /// </summary>
    [Fact]
    public void Law_Application_DoesNotDependOn_OuterLayers()
    {
        Application_DoesNotDependOn_OuterLayers(Namespaces.LawNamespace);
    }
}

