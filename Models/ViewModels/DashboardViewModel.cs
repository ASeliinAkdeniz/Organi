namespace Organi.Models.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public int UnreadMessages { get; set; }
        public int LowStockProducts { get; set; }
        public List<Organi.Models.Entities.Order> RecentOrders { get; set; } = new();
        public List<Organi.Models.Entities.ContactMessage> RecentMessages { get; set; } = new();
        public List<Organi.Models.Entities.AuditLog> RecentLogs { get; set; } = new();
    }
}

