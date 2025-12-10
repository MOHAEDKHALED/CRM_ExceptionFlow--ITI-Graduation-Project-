using System.ComponentModel.DataAnnotations;

namespace CRM_ExceptionFlow.DTOs.Auth
{
    public class LoginRequest
    {
        [Required, StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required, StringLength(128, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
    }
}

