using System.ComponentModel.DataAnnotations;

namespace PatriSystem.API.DTOs.Request
{
    public class ForgotPasswordRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}