namespace PatriSystem.API.DTOs.Response
{
    public class DashboardResponseDto
    {
        public int SalesToday { get; set; }
        public decimal TotalToday { get; set; }
        public int TotalProducts { get; set; }
        public int ActiveProducts { get; set; }
        public int InactiveProducts { get; set; }
        public int TotalSales { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}