namespace Player.Services.Interfaces
{
    public interface IFeedbackEmailService
    {
        Task SendEmail(string userEmail, string message);
    }
}
