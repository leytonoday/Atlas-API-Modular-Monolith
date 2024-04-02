namespace Atlas.Users.Application.CQRS.Authentication.Queries.Shared;

public record class CanSignInResponse(bool IsSuccess, Guid UserId, string UserName, string Email, IEnumerable<string> Roles);
