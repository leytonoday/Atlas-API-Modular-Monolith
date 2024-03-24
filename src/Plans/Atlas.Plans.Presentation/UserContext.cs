using Atlas.Plans.Presentation.Extensions;
using Atlas.Users.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Atlas.Plans.Presentation;

/// <inheritdoc/>
public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid UserId =>
        httpContextAccessor
            .HttpContext?
            .User
            .GetUserId() ??
        throw new ApplicationException("User context is unavailable");

    public bool IsAuthenticated =>
        httpContextAccessor
            .HttpContext?
            .User
            .Identity?
            .IsAuthenticated ??
        throw new ApplicationException("User context is unavailable");
}

