namespace PatriSystem.API.DTOs.Response
{
    public class SaleResponseDto
    {
        public Guid Id { get; set; }
        public int SaleNumber { get; set; }
        public string SaleNumberFormatted { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<SaleDetailResponseDto> SaleDetails { get; set; } = new();
    }
}