namespace Dome.Shared.Services.Interfaces.Authentication;

public interface ILoginService
{
    Task<ClientLoginFlow> CreateLoginFlow();

    Task<ClientSuccessfulNativeLogin> LoginUser(string email, string loginPassword, string flowId);
}
