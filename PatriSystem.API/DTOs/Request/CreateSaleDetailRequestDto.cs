using System.ComponentModel.DataAnnotations;

namespace PatriSystem.API.DTOs.Request
{
    public class CreateSaleDetailRequestDto
    {
        public Guid? ProductId { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public bool IsTemporary { get; set; } = false;

        public string? ProductName { get; set; }
    }
}