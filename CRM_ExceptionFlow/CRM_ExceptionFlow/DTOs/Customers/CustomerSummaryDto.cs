namespace CRM_ExceptionFlow.DTOs.Customers
{
    public class CustomerSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Company { get; set; }
        public string Status { get; set; } = "Active";
        public string? AssignedTo { get; set; }
        public int AssignedToUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

