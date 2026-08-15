using FluentValidation;
using PatriSystem.API.DTOs.Request;

namespace PatriSystem.API.Validators
{
    public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequestDto>
    {
        public ForgotPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo es requerido")
                .EmailAddress().WithMessage("Correo inválido");
        }
    }
}