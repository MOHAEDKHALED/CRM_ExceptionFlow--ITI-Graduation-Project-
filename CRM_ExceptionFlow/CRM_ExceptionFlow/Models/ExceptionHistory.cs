using System.ComponentModel.DataAnnotations;

namespace CRM_ExceptionFlow.Models
{
    public class ExceptionHistory
    {
        public int Id { get; set; }

        [Required]
        public int ExceptionId { get; set; }

        [Required]
        public string Status { get; set; }

        [Required, StringLength(100)]
        public string ChangedByUserName { get; set; }

        public string Notes { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Exception Exception { get; set; }
    }
}