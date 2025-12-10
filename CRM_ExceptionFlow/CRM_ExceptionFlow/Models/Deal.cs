using System.ComponentModel.DataAnnotations;

namespace CRM_ExceptionFlow.Models
{
    public class Deal
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Stage { get; set; } = "Prospecting";

        [Required]
        public string Priority { get; set; } = "Medium";

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int AssignedToUserId { get; set; }

        public DateTime? ExpectedCloseDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Customer Customer { get; set; }
        public virtual User AssignedToUser { get; set; }
    }
}