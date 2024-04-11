using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Atlas.Law.Infrastructure.Options;

public class GoogleTranslateOptionsSetup(IConfiguration configuration) : IConfigureOptions<GoogleTranslateOptions>
{
    private const string SectionName = "GoogleTranslateOptions";

    public void Configure(GoogleTranslateOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}

