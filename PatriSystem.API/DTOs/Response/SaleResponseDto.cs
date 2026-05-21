namespace PatriSystem.API.DTOs.Response
{
    public class SaleResponseDto
    {
        public Guid Id { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<SaleDetailResponseDto> SaleDetails { get; set; } = new();
    }
}