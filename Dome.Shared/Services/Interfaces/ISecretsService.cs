namespace Dome.Shared.Services.Interfaces
{
    public interface ISecretsService
    {
        // twilio
        public string? TwilioAccountSid { get; }
        public string? TwilioAccountAuthToken { get; }
        public string? TwilioVerifyServiceSid { get; }

        // sendgrid
        public string? SendGridAPIKey { get; }

        // aws s3
        public string? AWSS3AccessKey { get; }
        public string? AWSS3SecretAccessKey { get; }
        public string? AWSS3BucketName { get; }

        // mongoDB - player
        public string? RealmPlayerAppId { get; }

        public string? RealmAdminAppId { get; }

        public string? RealmVendorAppId { get; }

        // stripe
        public string? StripeSecretKey { get; }

        Task LoadAllSecrets();
    }
}
