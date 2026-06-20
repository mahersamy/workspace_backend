using WorkforcePortal.Application.DTOs.Auth;

namespace WorkforcePortal.Application.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task RegisterAsync(RegisterRequest request);
}
