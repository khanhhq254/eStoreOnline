namespace eStoreOnline.Infrastructure.Configurations;

public class StripeConfiguration
{
    public const string SectionName = "StripeConfiguration";

    public string ApiKey { get; set; } = string.Empty;

    public string WebhookSecret { get; set; } = string.Empty;
}