using System.ComponentModel.DataAnnotations;

namespace PatriSystem.API.DTOs.Request
{
    public class UpdateCategoryRequestDto
    {
        [Required(ErrorMessage = "El nombre de la categiria es requerido")]
        public string CategoryName { get; set; } = string.Empty;
    }
}
