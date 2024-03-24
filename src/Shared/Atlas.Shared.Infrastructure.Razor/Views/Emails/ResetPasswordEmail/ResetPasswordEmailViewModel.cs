namespace Atlas.Shared.Infrastructure.Razor.Views.Emails.ConfirmUserEmail;

public record ResetPasswordEmailViewModel(string UserName, string Token)
{
    public string Url { get; init; } = $"{Utils.GetWebsiteUrl()}auth/reset-password?token={Token}&username={UserName}";
}
