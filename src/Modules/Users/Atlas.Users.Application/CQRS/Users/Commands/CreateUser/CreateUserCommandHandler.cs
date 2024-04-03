using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Queue;
using Atlas.Users.Application.CQRS.Users.Queue.SendWelcomeEmail;
using Atlas.Users.Domain.Entities.UserEntity;
using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Application.CQRS.Users.Commands.CreateUser;

internal sealed class CreateUserCommandHandler(IUserRepository userRepository, UserManager<User> userManager, IQueueWriter queueWriter) : ICommandHandler<CreateUserCommand, string>
{
    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        User user = await User.CreateAsync(request.UserName, request.Email, request.Password, userManager);

        await userRepository.AddAsync(user, cancellationToken);
        
        // The sending of the email here could reasonably fail, and so it's best to move it into a queue so that it can be retries upon failure
        var sendEmail = new SendWelcomeEmailQueuedCommand(user.Id);
        await queueWriter.WriteAsync(sendEmail, cancellationToken);

        return user.Id.ToString();
    }
}
