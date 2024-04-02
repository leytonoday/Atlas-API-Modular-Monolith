using FluentValidation;

namespace Atlas.Users.Application.CQRS.Authentication.Queries.CanSignInWithToken;

public sealed class CanSignInWithTokenQueryValidator
: AbstractValidator<CanSignInWithTokenQuery>
{
    public CanSignInWithTokenQueryValidator()
    {
        RuleFor(command => command.Identifier)
            .NotEmpty();

        RuleFor(command => command.Token)
            .NotEmpty();
    }
}

