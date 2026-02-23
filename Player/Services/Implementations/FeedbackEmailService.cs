using System.Text;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Player.Services.Implementations
{
    public class FeedbackEmailService : IFeedbackEmailService
    {
        readonly StringBuilder emailString = new StringBuilder();

        private readonly IVersionTracking versionTracking;
        private readonly IDeviceInfo deviceInfo;
        private readonly ISecretsService secretsService;

        public FeedbackEmailService(
            IVersionTracking versionTracking,
            IDeviceInfo deviceInfo,
            ISecretsService secretsService
        )
        {
            this.versionTracking = versionTracking;
            this.deviceInfo = deviceInfo;
            this.secretsService = secretsService;
        }

        public async Task SendEmail(string userEmail, string message)
        {
            emailString.AppendLine($"User Email: {userEmail}, ");
            emailString.AppendLine($"Message: {message}, ");
            CreateVersionString();
            CreateDeviceInfoString();
            string? apiKey = secretsService.SendGridAPIKey;
            SendGridClient? client = new SendGridClient(apiKey);
            EmailAddress? from = new EmailAddress("information@jugtitech.com");
            string? subject = "Feedback from dome mobile app";
            EmailAddress? to = new EmailAddress("nithin.g@dafloinnovations.com");
            string? plainTextContent = emailString.ToString();
            string? htmlContent = emailString.ToString();
            SendGridMessage? msg = MailHelper.CreateSingleEmail(
                from,
                to,
                subject,
                plainTextContent,
                htmlContent
            );
            var response = await client.SendEmailAsync(msg);
        }

        private void CreateVersionString()
        {
            emailString.AppendLine("App Version Info - ");
            emailString.AppendLine($"Current Version:{versionTracking.CurrentVersion}, ");
            emailString.AppendLine($"Current Build:{versionTracking.CurrentBuild}, ");
            emailString.AppendLine(
                $"Version History:{string.Join(',', versionTracking.VersionHistory)}, "
            );
            emailString.AppendLine(
                $"Build History:{string.Join(',', versionTracking.BuildHistory)}, "
            );
        }

        private void CreateDeviceInfoString()
        {
            emailString.AppendLine("Device Info - ");
            emailString.AppendLine($"Model: {deviceInfo.Model}, ");
            emailString.AppendLine($"Manufacturer: {deviceInfo.Manufacturer}, ");
            emailString.AppendLine($"Name: {deviceInfo.Name}, ");
            emailString.AppendLine($"OS Version: {deviceInfo.VersionString}, ");
            emailString.AppendLine($"Idiom: {deviceInfo.Idiom}, ");
            emailString.AppendLine($"Platform: {deviceInfo.Platform},");
        }
    }
}
