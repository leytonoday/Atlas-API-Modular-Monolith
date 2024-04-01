using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Atlas.Shared.Infrastructure.Options.OptonsSetup;

public class EmailOptionsSetup(IConfiguration configuration) : IConfigureOptions<EmailOptions>
{
    private const string SectionName = "EmailOptions";

    public void Configure(EmailOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}
