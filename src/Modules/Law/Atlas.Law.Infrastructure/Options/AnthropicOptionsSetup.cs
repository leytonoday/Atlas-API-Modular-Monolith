using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Atlas.Law.Infrastructure.Options;

public class AnthropicOptionsSetup(IConfiguration configuration) : IConfigureOptions<AnthropicOptions>
{
    private const string SectionName = "AnthropicOptions";

    public void Configure(AnthropicOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}

