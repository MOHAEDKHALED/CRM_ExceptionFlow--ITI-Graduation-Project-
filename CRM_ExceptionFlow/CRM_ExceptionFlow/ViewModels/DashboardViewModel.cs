using CRM_ExceptionFlow.Models;

namespace CRM_ExceptionFlow.ViewModels
{

    public class DashboardViewModel
    {
        public int TotalCustomers { get; set; }
        public int ActiveCustomers { get; set; }
        public int TotalDeals { get; set; }
        public int ActiveDeals { get; set; }
        public decimal TotalRevenue { get; set; }
        public int OpenExceptions { get; set; }
        public int InProgressExceptions { get; set; }
        public int ResolvedExceptions { get; set; }
        public List<Models.Exception> RecentExceptions { get; set; }
        public List<Customer> TopCustomers { get; set; }
    }
}
