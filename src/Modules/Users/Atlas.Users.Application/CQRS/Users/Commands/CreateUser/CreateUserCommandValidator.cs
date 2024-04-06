using FluentValidation;

namespace Atlas.Users.Application.CQRS.Users.Commands.CreateUser;

internal sealed class CreateUserCommandValidator
    : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        // We don't need to verify if the username or email are unique here. There are database level constraints to prevent this.
        RuleFor(x => x.Email)
            .NotEmpty();

        RuleFor(x => x.UserName)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty();

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password);
    }
}