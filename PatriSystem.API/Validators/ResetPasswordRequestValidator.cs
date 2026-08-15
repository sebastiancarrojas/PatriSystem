using FluentValidation;
using PatriSystem.API.DTOs.Request;

namespace PatriSystem.API.Validators
{
    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequestDto>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("El token es requerido");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo es requerido")
                .EmailAddress().WithMessage("Correo inválido");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("La contraseña es requerida")
                .MinimumLength(8).WithMessage("Mínimo 8 caracteres")
                .Matches(@"[A-Z]").WithMessage("Debe contener al menos una mayúscula")
                .Matches(@"[a-z]").WithMessage("Debe contener al menos una minúscula")
                .Matches(@"\d").WithMessage("Debe contener al menos un número");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword).WithMessage("Las contraseñas no coinciden");
        }
    }
}