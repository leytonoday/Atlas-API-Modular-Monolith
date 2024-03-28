using FluentValidation;

namespace Atlas.Users.Application.CQRS.Users.Commands.ChangePassword;

public sealed class ChangePasswordCommandValidator
    : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty();

        RuleFor(x => x.NewPassword)
            .NotEmpty();
    }
}