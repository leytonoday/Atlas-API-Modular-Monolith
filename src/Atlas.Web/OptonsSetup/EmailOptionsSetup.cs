using Atlas.Shared.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Atlas.Web.OptonsSetup;

public class EmailOptionsSetup(IConfiguration configuration) : IConfigureOptions<EmailOptions>
{
    private const string SectionName = "EmailOptions";

    public void Configure(EmailOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}
