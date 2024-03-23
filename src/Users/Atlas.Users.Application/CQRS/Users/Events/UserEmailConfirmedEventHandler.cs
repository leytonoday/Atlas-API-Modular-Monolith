using Microsoft.AspNetCore.Identity;

namespace Atlas.Users.Application.CQRS.Users.Events;

//internal sealed class UserEmailConfirmedEventHandler(StripeService stripeService, UnitOfWork unitOfWork, UserManager<User> userManager) : IApplicationEventHandler<UserEmailConfirmedEvent>
//{
//    public async Task Handle(UserEmailConfirmedEvent notification, CancellationToken cancellationToken)
//    {
//        User user = await userManager.FindByIdAsync(notification.UserId.ToString())
//            ?? throw new ErrorException(InfrastructureErrors.User.UserNotFound);

//        var stripeCustomer = await stripeService.CreateCustomerAsync(user, cancellationToken);
//        await unitOfWork.StripeCustomerRepository.AddAsync(stripeCustomer, cancellationToken);

//        await unitOfWork.CommitAsync(cancellationToken);
//    }
//}
