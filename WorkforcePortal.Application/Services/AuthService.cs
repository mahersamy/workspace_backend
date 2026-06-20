using WorkforcePortal.Application.DTOs.Auth;
using WorkforcePortal.Application.Interfaces;
using WorkforcePortal.Application.Interfaces.Services;
using WorkforcePortal.Domain.Entities;
using WorkforcePortal.Domain.Exceptions;

namespace WorkforcePortal.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _uow;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IUnitOfWork uow, ITokenService tokenService, IPasswordHasher passwordHasher)
    {
        _uow = uow;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _uow.Users.GetUserByUsernameAsync(request.Username);
        if (user == null || !user.IsActive)
            throw new DomainException("Invalid username or password.");

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            throw new DomainException("Invalid username or password.");

        var token = _tokenService.GenerateToken(user);

        return new LoginResponse
        {
            Token = token,
            UserId = user.Id,
            Username = user.Username,
            Role = user.Role.ToString(),
            EmployeeId = user.EmployeeId
        };
    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _uow.Users.GetUserByUsernameAsync(request.Username);
        if (existingUser != null)
            throw new ConflictException("Username already exists.");

        var existingEmail = await _uow.Users.GetUserByEmailAsync(request.Email);
        if (existingEmail != null)
            throw new ConflictException("Email already exists.");

        if (request.EmployeeId.HasValue)
        {
            var employee = await _uow.Employees.GetByIdAsync(request.EmployeeId.Value);
            if (employee == null)
                throw new NotFoundException("Employee", request.EmployeeId.Value);
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            Role = request.Role,
            EmployeeId = request.EmployeeId,
            IsActive = true
        };

        await _uow.Users.AddAsync(user);
        await _uow.SaveChangesAsync();
    }
}
