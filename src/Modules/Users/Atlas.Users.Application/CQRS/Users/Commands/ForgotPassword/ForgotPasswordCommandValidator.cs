using FluentValidation;

namespace Atlas.Users.Application.CQRS.Users.Commands.ForgotPassword;

internal sealed class ForgotPasswordCommandValidator
    : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(x => x.Identifier)
            .NotEmpty();
    }
}