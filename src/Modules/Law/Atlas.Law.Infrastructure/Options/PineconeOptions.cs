namespace Atlas.Law.Infrastructure.Options;

public sealed class PineconeOptions
{
    public string ApiKey { get; set; } = null!;

    public string LegalDocumentIndexUrl { get; set; } = null!;
}

