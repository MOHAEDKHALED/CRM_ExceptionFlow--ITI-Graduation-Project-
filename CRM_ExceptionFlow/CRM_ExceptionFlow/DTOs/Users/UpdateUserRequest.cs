using System.ComponentModel.DataAnnotations;

namespace CRM_ExceptionFlow.DTOs.Users
{
    public class UpdateUserRequest
    {
        [Required, StringLength(256)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [RegularExpression("Admin|Manager|Employee")]
        public string Role { get; set; } = "Employee";

        [StringLength(50)]
        public string? Department { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(128, MinimumLength = 8)]
        public string? Password { get; set; }
    }
}

