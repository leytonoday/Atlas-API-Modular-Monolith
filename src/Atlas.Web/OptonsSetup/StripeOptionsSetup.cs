using Atlas.Plans.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Atlas.Web.OptonsSetup;

public class StripeOptionsSetup(IConfiguration configuration) : IConfigureOptions<StripeOptions>
{
    private const string SectionName = "StripeOptions";

    public void Configure(StripeOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}
