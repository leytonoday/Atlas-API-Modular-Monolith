using Atlas.Shared.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Atlas.Plans.Infrastructure.Options.OptionSetup;

public class StripeOptionsSetup(IConfiguration configuration) : IConfigureOptions<StripeOptions>
{
    private const string SectionName = "StripeOptions";

    public void Configure(StripeOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}
