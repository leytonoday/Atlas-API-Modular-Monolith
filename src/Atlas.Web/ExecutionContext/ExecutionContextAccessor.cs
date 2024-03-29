using Atlas.Shared.Application.Abstractions;
using System.Security.Claims;

namespace Atlas.Web.ExecutionContext;

/// <inheritdoc cref="IExecutionContextAccessor"/>
public class ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor) : IExecutionContextAccessor
{
    /// <inheritdoc/>
    public bool IsUserAuthenticated
    {
        get
        {
            if (!IsAvailable)
            {
                throw new UserContextNotAvailableException();
            }

            return httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }
    }

    /// <inheritdoc/>
    public Guid UserId
    {
        get
        {
            // Parse the UserId from a string in the User's claims
            string? userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is not null && Guid.TryParse(userId, out Guid parsedUserId))
            {
                return parsedUserId;
            }

            throw new UserContextNotAvailableException();
        }
    }

    /// <inheritdoc/>
    public bool IsAvailable => httpContextAccessor.HttpContext is not null;
}