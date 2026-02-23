namespace Dome.Shared.Services.Interfaces
{
    public interface ITwilioService
    {
        Task SendEmailOTP(string email);

        Task SendSMSOTP(string phoneNumber);

        Task<string> VerifyEmailOTP(string email, string otp);

        Task<string> VerifySMSOTP(string phoneNumber, string otp);
    }
}
