namespace Atlas.Shared.Infrastructure.Razor.Views.Emails.ConfirmUserEmail;

public record ConfirmUserEmailViewModel(string UserName, string Token)
{
    public string Url { get; init; } = $"{Utils.GetWebsiteUrl()}auth/verify-email?token={Token}&username={UserName}";
}
