using System.ComponentModel.DataAnnotations;

namespace PatriSystem.API.DTOs.Request
{
    public class UpdateProductRequestDto
    {
        [Required(ErrorMessage = "El nombre del producto es requerido")]
        public string ProductName { get; set; } = string.Empty;

        public string? Barcode { get; set; }

        public string? ProductDescription { get; set; }

        [Required(ErrorMessage = "La categoría es requerida")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "La marca es requerida")]
        public Guid BrandId { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal UnitPrice { get; set; }

        public string? UnitOfMeasure { get; set; }
    }
}