using CRM.Application.Common.Interfaces;
using CRM.Application.DTOs.Auth;
using CRM.Domain.Repositories;
using CRM.Domain.UnitOfWork;

namespace CRM.Application.UseCases.Auth;

public class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;
    
    public LoginUseCase(
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<LoginResponse?> ExecuteAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        
        if (user == null || !user.IsActive)
            return null;
        
        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            return null;
        
        user.RecordLogin();
        _userRepository.Update(user);
        
        var token = _jwtTokenService.GenerateToken(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        
        return new LoginResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddHours(2),
            User = new UserInfo
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role.Value
            }
        };
    }
}

