using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Users.Application.CQRS.Authentication.Queries.Shared;

namespace Atlas.Users.Application.CQRS.Authentication.Queries.CanSignIn;

public record CanSignInQuery(string Identifier, string Password) : IQuery<CanSignInResponse>;
