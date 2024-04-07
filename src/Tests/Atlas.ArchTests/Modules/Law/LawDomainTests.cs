using Atlas.ArchTests.Shared;
using Atlas.Law.Domain;

namespace Atlas.ArchTests.Modules.Law;

public class LawDomainTests : BaseDomainTests
{
    public LawDomainTests() : base(typeof(LawDomainAssemblyReference).Assembly) { }
}
