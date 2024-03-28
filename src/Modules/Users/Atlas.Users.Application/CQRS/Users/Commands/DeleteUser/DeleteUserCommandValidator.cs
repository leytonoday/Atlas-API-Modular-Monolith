using FluentValidation;

namespace Atlas.Users.Application.CQRS.Users.Commands.DeleteUser;

public sealed class DeleteUserCommandValidator
    : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty();
    }
}