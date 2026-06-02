namespace PatriSystem.API.DTOs.Response
{
    public class TopProductResponseDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Revenue { get; set; }
    }
}
