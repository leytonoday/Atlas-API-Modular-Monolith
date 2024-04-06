using FluentValidation;

namespace Atlas.Users.Application.CQRS.Users.Commands.UpdateUser;

internal sealed class UpdateUserCommandValidator
    : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty();
    }
}