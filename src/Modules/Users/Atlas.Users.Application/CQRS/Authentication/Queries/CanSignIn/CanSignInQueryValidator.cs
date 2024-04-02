using FluentValidation;

namespace Atlas.Users.Application.CQRS.Authentication.Queries.CanSignIn;

public sealed class CanSignInQueryValidator
: AbstractValidator<CanSignInQuery>
{
    public CanSignInQueryValidator()
    {
        RuleFor(command => command.Identifier)
            .NotEmpty();

        RuleFor(command => command.Password)
            .NotEmpty();
    }
}

