using System.ComponentModel.DataAnnotations;

namespace PatriSystem.API.DTOs.Request
{
    public class CreateUnitOfMeasureRequestDto
    {
        [Required(ErrorMessage = "El nombre de la unidad de medida es requerido")]
        public string Name { get; set; } = string.Empty;
    }
}