using CRM.Domain.Entities;

namespace CRM.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
}


