namespace eStoreOnline.Infrastructure.Configurations;

public class EmailSenderConfiguration
{
    public const string SectionName = "EmailSenderConfiguration";
    
    public string SendGridKey { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
}