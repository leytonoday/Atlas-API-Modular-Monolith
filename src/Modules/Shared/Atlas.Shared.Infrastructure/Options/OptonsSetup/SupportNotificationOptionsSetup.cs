using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Atlas.Shared.Infrastructure.Options.OptonsSetup;

public class SupportNotificationOptionsSetup(IConfiguration configuration) : IConfigureOptions<SupportNotificationOptions>
{
    private const string SectionName = "SupportNotificationOptions";

    public void Configure(SupportNotificationOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}
