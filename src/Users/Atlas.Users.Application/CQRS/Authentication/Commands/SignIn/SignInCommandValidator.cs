using FluentValidation;

namespace Atlas.Users.Application.CQRS.Authentication.Commands.SignIn;

public sealed class SignInCommandValidator
    : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(command => command.Identifier)
            .NotEmpty();

        RuleFor(command => command.Password)
            .NotEmpty();
    }
}

