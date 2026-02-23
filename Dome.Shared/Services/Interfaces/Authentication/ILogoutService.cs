using System;

namespace Dome.Shared.Services.Interfaces.Authentication;

public interface ILogoutService
{
    Task LogoutUser();
}
