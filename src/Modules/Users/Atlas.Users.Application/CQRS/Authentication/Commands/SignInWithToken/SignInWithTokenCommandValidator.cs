using FluentValidation;

namespace Atlas.Users.Application.CQRS.Authentication.Commands.SignInWithToken;

public sealed class SignInCommandValidator
    : AbstractValidator<SignInWithTokenCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(command => command.Identifier)
            .NotEmpty();

        RuleFor(command => command.Token)
            .NotEmpty();
    }
}

