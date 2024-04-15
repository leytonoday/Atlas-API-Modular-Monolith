namespace Atlas.Shared;

/// <summary>
/// Provides utility methods used throughout the application.
/// </summary>
public static class Utils
{
    /// <summary>
    /// Determines whether the current environment is development.
    /// </summary>
    /// <returns><c>true</c> if the current environment is Development; otherwise, <c>false</c>.</returns>
    public static bool IsDevelopment()
    {
        return string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "development", StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Gets the URL of the website based on the current environment.
    /// </summary>
    /// <returns>The URL of the website.</returns>
    public static string GetWebsiteUrl()
    {
        bool isDevelopment = IsDevelopment();
        return isDevelopment ? Constants.WebsiteUrlDev : Constants.WebsiteUrlProd;
    }
}