using FluentValidation;
using PatriSystem.API.DTOs.Request;

namespace PatriSystem.API.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo es requerido")
                .EmailAddress().WithMessage("Correo inválido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es requerida");
        }
    }
}