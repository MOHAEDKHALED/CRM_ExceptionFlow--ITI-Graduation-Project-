namespace CRM_ExceptionFlow.DTOs.Exceptions
{
    public class ExceptionHistoryDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ChangedByUserName { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}

