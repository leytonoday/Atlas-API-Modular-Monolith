using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using PhoneNumbers;
using Atlas.Users.Domain.Errors;
using Atlas.Users.Domain.Extensions;
using Atlas.Shared.Domain.Entities;
using Atlas.Shared.Domain.AggregateRoot;
using Atlas.Shared.Domain.Events;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Shared.Domain.Events.UserEvents;
using Atlas.Shared;

namespace Atlas.Users.Domain.Entities.UserEntity;

public sealed class User : IdentityUser<Guid>, IEntity<Guid>, IAuditableEntity, IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <inheritdoc />
    [NotMapped]
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.ToList();

    /// <inheritdoc />
    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    /// <inheritdoc />
    public void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);

    /// <inheritdoc />
    public void ClearDomainEvent(IDomainEvent domainEvent) => _domainEvents.Clear();

    /// <inheritdoc />
    public DateTime CreatedOnUtc { get; init; }

    /// <inheritdoc />
    public DateTime? UpdatedOnUtc { get; init; }

    public Guid? CustomerId { get; private set; } = null;

    /// <summary>
    /// Represents the <see cref="Plan"/> that the user is subscribed to. 
    /// </summary>
    public Guid? PlanId { get; private set; }

    public static void SetCustomer(User user, Guid customerId)
    {
        user.CustomerId = customerId;
    }

    public static async Task<User> CreateAsync(string userName, string email, string password, UserManager<User> userManager)
    {
        var user = new User()
        {
            UserName = userName,
            Email = email,
        };

        user.AddDomainEvent(new UserCreatedEvent(Guid.NewGuid(), email));

        IdentityResult result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            throw new ErrorException(result.GetErrors());
        }

        return user;
    }

    public static async Task UpdateAsync(
        User user, 
        string userName, 
        string? phoneNumber, 
        UserManager<User> userManager)
    {
        // If the user has changed username, ensure it hasn't been taken already
        if (user.UserName != userName)
        {
            User? existingUser = await userManager.FindByUserNameOrEmailAsync(userName);
            if (existingUser is not null) 
            {
                throw new ErrorException(UsersDomainErrors.User.UserNameAlreadyInUse);
            }
        }

        // Validate phonenumber
        if (phoneNumber is not null && user.PhoneNumber != phoneNumber && !PhoneNumberUtil.IsViablePhoneNumber(phoneNumber))
        {
            throw new ErrorException(UsersDomainErrors.PhoneNumber.Invalid);
        }

        user.UserName = userName;
        user.PhoneNumber = phoneNumber;

        user.AddDomainEvent(new UserUpdatedEvent(Guid.NewGuid(), user.Id));
        await userManager.UpdateAsync(user);
    }

    public static async Task<User> DeleteUserAsync(
        User currentUser,
        Guid userId,
        string password,
        UserManager<User> userManager)
    {
        User userToBeDeleted = await userManager.FindByIdAsync(userId.ToString())
            ?? throw new ErrorException(UsersDomainErrors.User.UserNotFound);

        // Is the user making the request the same user that is being requested to be deleted?
        bool isDeletingSelf = userId == currentUser.Id;

        // Is the user making the request an Admin?
        bool isCurrentUserAdmin = await userManager.IsInRoleAsync(currentUser, RoleNames.Administrator);

        // Is the user that is being requested to be deleted an Admin?
        bool isToDeleteUserAdmin = await userManager.IsInRoleAsync(userToBeDeleted, RoleNames.Administrator);

        // If an Admin is trying to delete another Admin
        if (!isDeletingSelf && isCurrentUserAdmin && isToDeleteUserAdmin)
        {
            throw new ErrorException(UsersDomainErrors.User.AdminCanNotDeleteOtherAdmins);
        }
        // If a NON-ADMIN user is trying to delete a user that is NOT themselves
        else if (!isCurrentUserAdmin && !isDeletingSelf)
        {
            throw new ErrorException(UsersDomainErrors.User.NonAdminCanOnlyDeleteOwnAccount);
        }
        // If an Admin is trying to delete another NON-ADMIN user
        else if (isCurrentUserAdmin && !isToDeleteUserAdmin && !isDeletingSelf)
        {
            // An admin can delete another non-admin user without a password, so proceed with deletion
            userToBeDeleted.AddDomainEvent(new UserDeletedEvent(Guid.NewGuid(), userToBeDeleted.Id));
            await userManager.DeleteAsync(userToBeDeleted);
        }
        // If a user is deleting themselves, regardless of if they're an Admin or not, they have to provide their password
        else if (isDeletingSelf)
        {
            var passwordHasher = new PasswordHasher<User>();
            // Verify the password provided is correct. 
            if (string.IsNullOrWhiteSpace(password) || passwordHasher.VerifyHashedPassword(currentUser, currentUser.PasswordHash, password) != PasswordVerificationResult.Success)
            {
                throw new ErrorException(UsersDomainErrors.User.InvalidCredentials);
            }

            // Password has been provided successfully, so proceed with deletion
            userToBeDeleted.AddDomainEvent(new UserDeletedEvent(Guid.NewGuid(), userToBeDeleted.Id));
            await userManager.DeleteAsync(userToBeDeleted);
        }


        return userToBeDeleted;
    }

    public static void SetPlanId(User user, Guid? planId)
    {
        user.PlanId = planId;
    }
}