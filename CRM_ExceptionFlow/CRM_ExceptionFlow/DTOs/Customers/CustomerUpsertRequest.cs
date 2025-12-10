using System.ComponentModel.DataAnnotations;

namespace CRM_ExceptionFlow.DTOs.Customers
{
    public class CustomerUpsertRequest
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [EmailAddress, StringLength(100)]
        public string? Email { get; set; }

        [Phone, StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(100)]
        public string? Company { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [Required]
        public string Status { get; set; } = "Active";

        [Required]
        public int AssignedToUserId { get; set; }
    }
}

