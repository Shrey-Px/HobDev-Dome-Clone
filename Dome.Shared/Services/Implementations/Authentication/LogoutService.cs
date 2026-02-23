using System;

namespace Dome.Shared.Services.Implementations.Authentication;

public class LogoutService : ILogoutService
{
    private readonly FrontendApi _frontendApi;
    private readonly ISecureStorage _secureStorage;

    public LogoutService(ISecureStorage secureStorage)
    {
        var configuration = new Configuration { BasePath = AppConstants.BaseUrl };

        _frontendApi = new FrontendApi(configuration);
        _secureStorage = secureStorage;
    }

    public async Task LogoutUser()
    {
        string? sessionToken = await _secureStorage.GetAsync("sessionToken");

        ClientPerformNativeLogoutBody? clientPerformNativeLogoutBody =
            new ClientPerformNativeLogoutBody(sessionToken: sessionToken);
        await _frontendApi.PerformNativeLogoutAsync(clientPerformNativeLogoutBody);
    }
}
