using System.ComponentModel.DataAnnotations;

namespace CRM_ExceptionFlow.Models
{
    public class Lead
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; }

        [Phone, StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(100)]
        public string? Company { get; set; }

        [StringLength(50)]
        public string Source { get; set; } = "Website"; // Website, Referral, Cold Call, Social Media, Event

        [Required, StringLength(50)]
        public string Status { get; set; } = "New"; // New, Contacted, Qualified, Converted, Lost

        [Required, StringLength(50)]
        public string Quality { get; set; } = "Warm"; // Hot, Warm, Cold

        public string? Notes { get; set; }

        public int? AssignedToUserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ContactedAt { get; set; }

        public DateTime? ConvertedAt { get; set; }

        public int? ConvertedToCustomerId { get; set; }

        // Navigation Properties
        public virtual User? AssignedToUser { get; set; }
        public virtual Customer? ConvertedToCustomer { get; set; }
    }
}
