using Atlas.Shared.Domain.Errors;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Domain.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="IdentityResult"/> class.
/// </summary>
public static class IdentityResultExtensions
{
    /// <summary>
    /// Gets the errors from the IdentityResult, or returns an unknown error if no specific errors are present.
    /// </summary>
    /// <param name="result">The IdentityResult to extract errors from.</param>
    /// <returns>An array of <see cref="Error"/> representing the errors in the IdentityResult.</returns>
    public static IEnumerable<Error> GetErrors(this IdentityResult result)
    {
        // if there's no errors, despite the IdentityResult failing, then simply say it's an unknown error
        if (result.Errors is null || !result.Errors.Any())
        {
            return new[] { Error.UnknownError };
        }

        return result.Errors.Select(x => new Error(x.Code, x.Description));
    }
}
