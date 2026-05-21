namespace PatriSystem.API.DTOs.Response
{
    public class BrandResponseDto
    {
        public Guid Id { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string? BrandDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}