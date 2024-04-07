using ArchTests.Shared;
using Atlas.Law.Application;
using Atlas.Law.Domain;
using Atlas.Law.Infrastructure;
using System.Reflection;
using Atlas.ArchTests.Shared;

namespace Atlas.ArchTests.Modules.Law;

public class LawModuleTests : BaseModuleTests
{
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

