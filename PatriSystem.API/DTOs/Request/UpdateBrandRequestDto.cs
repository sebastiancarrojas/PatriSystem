using System.ComponentModel.DataAnnotations;

namespace PatriSystem.API.DTOs.Request
{
    public class UpdateBrandRequestDto
    {
        [Required(ErrorMessage = "El nombre de la marca es requerido")]
        public string BrandName { get; set; } = string.Empty;
        public string BrandDescription { get; set; } = string.Empty;
    }
}
