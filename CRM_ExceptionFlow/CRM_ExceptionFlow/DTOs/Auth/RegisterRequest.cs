using System.ComponentModel.DataAnnotations;

namespace CRM_ExceptionFlow.DTOs.Auth
{
    public class RegisterRequest
    {
        [Required, StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(256)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [RegularExpression("Admin|Manager|Employee")]
        public string Role { get; set; } = "Employee";

        [StringLength(50)]
        public string? Department { get; set; }

        [Required, StringLength(128, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}

