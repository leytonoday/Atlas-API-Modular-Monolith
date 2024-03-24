using Atlas.Shared.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Atlas.Web.OptonsSetup;

public class SupportNotificationOptionsSetup(IConfiguration configuration) : IConfigureOptions<SupportNotificationOptions>
{
    private const string SectionName = "SupportNotificationOptions";

    public void Configure(SupportNotificationOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}
