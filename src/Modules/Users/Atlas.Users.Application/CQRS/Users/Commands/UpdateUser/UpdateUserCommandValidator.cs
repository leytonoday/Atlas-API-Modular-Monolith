using FluentValidation;

namespace Atlas.Users.Application.CQRS.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandValidator
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