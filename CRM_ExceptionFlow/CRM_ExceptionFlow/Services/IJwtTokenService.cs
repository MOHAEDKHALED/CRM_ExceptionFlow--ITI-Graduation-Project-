using CRM_ExceptionFlow.Models;

namespace CRM_ExceptionFlow.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}

