namespace PatriSystem.API.DTOs.Response
{
    public class CategoryResponseDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}