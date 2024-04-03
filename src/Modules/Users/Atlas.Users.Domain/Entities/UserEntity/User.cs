using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using PhoneNumbers;
using Atlas.Users.Domain.Errors;
using Atlas.Users.Domain.Extensions;
using Atlas.Shared.Domain.Entities;
using Atlas.Shared.Domain.AggregateRoot;
using Atlas.Shared.Domain.Events;
using Atlas.Shared.Domain.Exceptions;
using Atlas.Shared;
using Atlas.Users.Domain.Entities.UserEntity.Events;
using Atlas.Shared.Domain.BusinessRules;
using Atlas.Users.Domain.Entities.UserEntity.BusinessRules;
using System.Buffers.Text;

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
    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <summary>
    /// Ensures a given business rule has not been broken.
    /// </summary>
    /// <param name="rule">The business rule to ensure isn't broken.</param>
    /// <exception cref="BusinessRuleBrokenException" />
    static void CheckBusinessRule(IBusinessRule rule)
    {
        if (rule.IsBroken()) throw new BusinessRuleBrokenException(rule.Message);
    }

    /// <summary>
    /// Ensures a given asynchronous business rule has not been broken.
    /// </summary>
    /// <param name="rule">The business rule to ensure isn't broken.</param>
    /// <exception cref="BusinessRuleBrokenException" />
    static async Task CheckAsyncBusinessRule(IAsyncBusinessRule rule, CancellationToken cancellationToken = default)
    {
        if (await rule.IsBrokenAsync(cancellationToken)) throw new BusinessRuleBrokenException(rule.Message);
    }

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
            Id = Guid.NewGuid(),
            UserName = userName,
            Email = email,
        };

        // Username Validation
        CheckBusinessRule(new UserNameMustUseAllowedCharactersBusinessRule(userName));
        await CheckAsyncBusinessRule(new UserNameMustBeUniqueBusinessRule(userName, userManager));

        // Email validation
        await CheckAsyncBusinessRule(new EmailMustBeUniqueBusinessRule(email, userManager));

        // Password Validation
        CheckBusinessRule(new PasswordMustHaveDigitsBusinessRule(password));
        CheckBusinessRule(new PasswordMustHaveLowerCaseBusinessRule(password));
        CheckBusinessRule(new PasswordMustHaveUpperCaseBusinessRule(password));
        CheckBusinessRule(new PasswordMustHaveNonAlphanumericLettersBusinessRule(password));
        CheckBusinessRule(new PasswordMustBeMinimumLengthBusinessRule(password));

        user.PasswordHash = userManager.PasswordHasher.HashPassword(user, password);
        user.NormalizedEmail = userManager.NormalizeEmail(email);
        user.NormalizedUserName = userManager.NormalizeName(userName);
        user.SecurityStamp = userManager.GenerateNewAuthenticatorKey();

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

        user.AddDomainEvent(new UserUpdatedDomainEvent(user.Id));

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
            await userManager.DeleteAsync(userToBeDeleted);
        }

        // Add domain event to allow domain to react 
        userToBeDeleted.AddDomainEvent(new UserDeletedDomainEvent(userToBeDeleted.Id));

        return userToBeDeleted;
    }

    public static void SetPlanId(User user, Guid? planId)
    {
        user.PlanId = planId;
    }
}