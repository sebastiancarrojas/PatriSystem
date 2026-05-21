using System.ComponentModel.DataAnnotations;

namespace PatriSystem.API.DTOs.Request
{
    public class CreateSaleRequestDto
    {
        [Required(ErrorMessage = "La venta debe tener al menos un producto")]
        [MinLength(1, ErrorMessage = "La venta debe tener al menos un producto")]
        public List<CreateSaleDetailRequestDto> Details { get; set; } = new();
    }
}