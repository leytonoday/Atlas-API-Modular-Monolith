using System.Security.Claims;

namespace Atlas.Plans.Presentation.Extensions;

/// <summary>
/// Extension methods for working with <see cref="ClaimsPrincipal"/> related operations.
/// </summary>
internal static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Gets the user ID from the specified <see cref="ClaimsPrincipal"/>.
    /// </summary>
    /// <param name="claimsPrincipal">The <see cref="ClaimsPrincipal"/> from which to retrieve the user ID.</param>
    /// <returns>The user ID as a <see cref="Guid"/>.</returns>
    /// <exception cref="ApplicationException">Thrown when the user ID is unavailable or cannot be parsed.</exception>
    public static Guid GetUserId(this ClaimsPrincipal? claimsPrincipal)
    {
        string? userId = claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userId, out Guid parsedUserId) ?
            parsedUserId :
            throw new ApplicationException("User id is unavailable");
    }

    /// <summary>
    /// Gets the customer ID from the specified <see cref="ClaimsPrincipal"/>.
    /// </summary>
    /// <param name="claimsPrincipal">The <see cref="ClaimsPrincipal"/> from which to retrieve the customer ID.</param>
    /// <returns>The customer ID as a <see cref="Guid"/>.</returns>
    /// <exception cref="ApplicationException">Thrown when the customer ID is unavailable or cannot be parsed.</exception>
    public static Guid GetCustomerId(this ClaimsPrincipal? claimsPrincipal)
    {
        string? customerId = claimsPrincipal?.FindFirstValue("CustomerId");

        return Guid.TryParse(customerId, out Guid parsedCustomerId) ?
            parsedCustomerId :
            throw new ApplicationException("Customer id is unavailable");
    }
}
