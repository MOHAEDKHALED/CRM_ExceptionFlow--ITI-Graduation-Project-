namespace CRM_ExceptionFlow.DTOs.Dashboard
{
    public class DashboardSummaryDto
    {
        public int TotalCustomers { get; set; }
        public int ActiveDeals { get; set; }
        public int OpenExceptions { get; set; }
        public int UpcomingInteractions { get; set; }
    }
}

