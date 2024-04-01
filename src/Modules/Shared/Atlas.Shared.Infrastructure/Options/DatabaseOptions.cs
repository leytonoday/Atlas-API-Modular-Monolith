namespace Atlas.Shared.Infrastructure.Options;

/// <summary>
/// Represents the options for the database and Entity Framework Core, which are loaded from the appsettings.json file.
/// </summary>
public class DatabaseOptions
{
    /// <summary>
    /// The amount of seconds to wait for a query to be executed before abandoning it.
    /// </summary>
    public int CommandTimeout { get; set; }

    /// <inheritdoc cref="DbContextOptionsBuilder.EnableDetailedErrors(bool)" />
    public bool EnableDetailedErrors { get; set; }


    /// <inheritdoc cref="DbContextOptionsBuilder.EnableSensitiveDataLogging(bool)" />
    public bool EnableSensitiveDataLogging { get; set; }
}
