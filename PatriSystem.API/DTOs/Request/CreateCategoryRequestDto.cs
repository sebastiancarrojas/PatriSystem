using System.ComponentModel.DataAnnotations;

namespace PatriSystem.API.DTOs.Request
{
    public class CreateCategoryRequestDto
    {
        [Required(ErrorMessage = "El nombre de la categoría es requerido")]
        public string CategoryName { get; set; } = string.Empty;
    }
}