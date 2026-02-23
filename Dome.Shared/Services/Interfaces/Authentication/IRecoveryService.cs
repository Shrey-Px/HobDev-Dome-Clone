using System;

namespace Dome.Shared.Services.Interfaces.Authentication;

public interface IRecoveryService
{
    Task<ClientRecoveryFlow> CreateRecoveryFlow();
    Task<ClientRecoveryFlow> RecoverPassword(string email, string flowId);
}
