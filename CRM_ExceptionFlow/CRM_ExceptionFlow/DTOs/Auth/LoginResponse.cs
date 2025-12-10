using CRM_ExceptionFlow.DTOs.Users;

namespace CRM_ExceptionFlow.DTOs.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public UserDto User { get; set; } = new();
    }
}

