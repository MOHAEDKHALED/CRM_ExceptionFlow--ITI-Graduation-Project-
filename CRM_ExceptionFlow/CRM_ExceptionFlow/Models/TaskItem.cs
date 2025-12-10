using System.ComponentModel.DataAnnotations;

namespace CRM_ExceptionFlow.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required, StringLength(50)]
        public string Type { get; set; } = "Call"; // Call, Email, Meeting, Follow-up, Demo

        [Required, StringLength(50)]
        public string Priority { get; set; } = "Medium"; // High, Medium, Low

        [Required]
        public DateTime DueDate { get; set; }

        [Required, StringLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, In Progress, Completed, Cancelled

        [Required]
        public int AssignedToUserId { get; set; }

        public int? CustomerId { get; set; }

        public int? DealId { get; set; }

        public int? LeadId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }

        public string? CompletionNotes { get; set; }

        // Navigation Properties
        public virtual User AssignedToUser { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual Deal? Deal { get; set; }
        public virtual Lead? Lead { get; set; }
    }
}
