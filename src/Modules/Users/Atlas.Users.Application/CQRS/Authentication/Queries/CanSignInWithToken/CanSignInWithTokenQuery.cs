using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Abstractions.Messaging.Query;
using Atlas.Users.Application.CQRS.Authentication.Queries.Shared;

namespace Atlas.Users.Application.CQRS.Authentication.Queries.CanSignInWithToken;

public sealed record CanSignInWithTokenQuery(string Identifier, string Token) : IQuery<CanSignInResponse>;
