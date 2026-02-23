namespace Dome.Shared.Services.Implementations
{
    public class TwilioService : ITwilioService
    {
        readonly string? verifyServiceSid;

        public TwilioService(ISecretsService secretsService)
        {
            string? accountSid = secretsService?.TwilioAccountSid;
            string? authToken = secretsService?.TwilioAccountAuthToken;
            verifyServiceSid = secretsService?.TwilioVerifyServiceSid;

            //To take advantage of Twilio's Global Infrastructure, specify the target Region
            // TwilioClient.SetRegion("us1");
            TwilioClient.Init(accountSid, authToken);
        }

        public async Task SendEmailOTP(string email)
        {
            VerificationResource verification = VerificationResource.Create(
                to: email,
                channel: "email",
                pathServiceSid: verifyServiceSid
            );
        }

        public async Task<string> VerifyEmailOTP(string email, string otp)
        {
            VerificationCheckResource verificationCheck = VerificationCheckResource.Create(
                to: email,
                code: otp,
                pathServiceSid: verifyServiceSid
            );

            return verificationCheck.Status;
        }

        public async Task SendSMSOTP(string phoneNumber)
        {
            var verification = VerificationResource.Create(
                to: phoneNumber,
                channel: "sms",
                pathServiceSid: verifyServiceSid
            );
        }

        public async Task<string> VerifySMSOTP(string phoneNumber, string otp)
        {
            var verificationCheck = VerificationCheckResource.Create(
                to: phoneNumber,
                code: otp,
                pathServiceSid: verifyServiceSid
            );
            return verificationCheck.Status;
        }
    }
}
