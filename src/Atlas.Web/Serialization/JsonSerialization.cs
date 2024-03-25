using System.Text.Json.Serialization;
using System.Text.Json;

namespace Atlas.Web.Serialization;

/// <summary>
/// Provides static properties for JSON serialization.
/// </summary>
public static class JsonSerialization
{
    /// <summary>
    /// Specifies whether the JSON serializer should write indented JSON. This is <c>true</c> in debug builds, and <c>false</c> in release builds.
    /// </summary>
    public const bool WriteIndented =
#if DEBUG
            true
#else
            false
#endif
        ;

    /// <summary>
    /// Gets the naming policy for JSON serialization.
    /// </summary>
    public static JsonNamingPolicy NamingPolicy { get; } = JsonNamingPolicy.CamelCase;

    /// <summary>
    /// Gets the JSON serializer options.
    /// </summary>
    public static JsonSerializerOptions Options { get; } = new()
    {
        PropertyNamingPolicy = NamingPolicy,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
    };

    /// <summary>
    /// Gets the options for JSON writer.
    /// </summary>
    public static JsonWriterOptions WriterOptions { get; } = new()
    {
        Indented = WriteIndented
    };
}
