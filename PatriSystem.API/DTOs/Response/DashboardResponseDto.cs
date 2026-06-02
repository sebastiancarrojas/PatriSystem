namespace PatriSystem.API.DTOs.Response
{
    public class DashboardResponse
    {
        public int SalesTodayCount { get; set; }
        public decimal SalesTodayAmount { get; set; }
        public int SalesWeekCount { get; set; }
        public decimal SalesWeekAmount { get; set; }
        public int SalesMonthCount { get; set; }
        public decimal SalesMonthAmount { get; set; }
        public int TotalProducts { get; set; }
        public IEnumerable<DailySalesResponseDto> Last7DaysSales { get; set; } = new List<DailySalesResponseDto>();
        public IEnumerable<TopProductResponseDto> TopProductsLastMonth { get; set; } = new List<TopProductResponseDto>();
    }
}