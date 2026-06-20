using WorkforcePortal.Domain.Entities;

namespace WorkforcePortal.Application.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}
