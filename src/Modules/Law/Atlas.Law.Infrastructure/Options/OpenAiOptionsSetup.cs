using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Atlas.Law.Infrastructure.Options;

public class OpenAiOptionsSetup(IConfiguration configuration) : IConfigureOptions<OpenAiOptions>
{
    private const string SectionName = "OpenAiOptions";

    public void Configure(OpenAiOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}
