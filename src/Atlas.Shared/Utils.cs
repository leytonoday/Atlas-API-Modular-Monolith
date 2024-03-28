namespace Atlas.Shared;

public static class Utils
{
    /// <summary>
    /// Determines whether the current environment is development or not.
    /// </summary>
    /// <returns><c>true</c> if currently in Development mode, and <c>false</c> if in Production.</returns>
    public static bool IsDevelopment()
    {
        return string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "development", StringComparison.InvariantCultureIgnoreCase);
    }

    public static string GetWebsiteUrl()
    {
        bool isDevelopment = Utils.IsDevelopment();
        return isDevelopment ? Constants.WebsiteUrlDev : Constants.WebsiteUrlProd;
    }
}
