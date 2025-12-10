using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace CRM_ExceptionFlow.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [EmailAddress, StringLength(100)]
        public string Email { get; set; }

        [Phone, StringLength(20)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string Company { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        public string Status { get; set; } = "Active";

        [Required]
        public int AssignedToUserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual User AssignedToUser { get; set; }
        public virtual ICollection<Deal> Deals { get; set; }
        public virtual ICollection<Interaction> Interactions { get; set; }
    }
}