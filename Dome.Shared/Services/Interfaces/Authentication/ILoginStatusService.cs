using System;

namespace Dome.Shared.Services.Interfaces.Authentication;

public interface ILoginStatusService
{
    bool IsLoggedIn { get; set; }
}
