using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Application.CQRS.Users.Events;

//internal sealed class UserUpdatedEventHandler(StripeService stripeService, UserManager<User> userManager) : IApplicationEventHandler<UserUpdatedEvent>
//{
//    public async Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
//    {
//        User user = await userManager.FindByIdAsync(notification.UserId.ToString())
//            ?? throw new ErrorException(InfrastructureErrors.User.UserNotFound);

//        await stripeService.UpdateCustomerAsync(user, cancellationToken);
//    }
//}
