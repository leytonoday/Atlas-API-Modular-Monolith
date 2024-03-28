using FluentValidation;

namespace Atlas.Users.Application.CQRS.Users.Commands.RefreshConfirmUserEmail;

public sealed class RefreshConfirmUserEmailCommandValidator
    : AbstractValidator<RefreshConfirmUserEmailCommand>
{
    public RefreshConfirmUserEmailCommandValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty();
    }
}