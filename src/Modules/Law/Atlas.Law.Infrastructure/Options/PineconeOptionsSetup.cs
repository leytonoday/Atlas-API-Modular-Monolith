using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Atlas.Law.Infrastructure.Options;

public class PineconeOptionsSetup(IConfiguration configuration) : IConfigureOptions<PineconeOptions>
{
    private const string SectionName = "PineconeOptions";

    public void Configure(PineconeOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}
