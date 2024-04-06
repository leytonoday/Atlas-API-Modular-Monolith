using FluentValidation;

namespace Atlas.Users.Application.CQRS.Users.Commands.ConfirmUserEmail;

internal sealed class ConfirmUserEmailCommandValidator
: AbstractValidator<ConfirmUserEmailCommand>
{
    public ConfirmUserEmailCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();

        RuleFor(x => x.UserName)
            .NotEmpty();
    }
}