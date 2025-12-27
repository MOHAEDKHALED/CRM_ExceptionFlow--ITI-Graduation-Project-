using CRM.Application.Common.Interfaces;
using CRM.Application.DTOs.Auth;
using CRM.Domain.Entities;
using CRM.Domain.Repositories;
using CRM.Domain.UnitOfWork;
using CRM.Domain.ValueObjects;

namespace CRM.Application.UseCases.Auth;

public class RegisterUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;
    
    public RegisterUseCase(
        IUserRepository userRepository,
        IJwtTokenService jwtTokenService,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<LoginResponse?> ExecuteAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (await _userRepository.UsernameExistsAsync(request.Username, cancellationToken))
            throw new InvalidOperationException("Username already exists");
        
        if (await _userRepository.EmailExistsAsync(request.Email, cancellationToken))
            throw new InvalidOperationException("Email already exists");
        
        var passwordHash = _passwordHasher.HashPassword(request.Password);
        var role = UserRole.FromString(request.Role);
        
        var user = new User(
            request.Username,
            request.Email,
            passwordHash,
            request.FullName,
            role,
            request.Department);
        
        _userRepository.Add(user);
        
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

