using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PatriSystem.Domain.Common;
using PatriSystem.Domain.Entities;
using PatriSystem.Domain.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PatriSystem.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IEmailSender _emailSender;
        private readonly FrontendSettings _frontendSettings;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IOptions<JwtSettings> jwtSettings,
            IEmailSender emailSender,
            IOptions<FrontendSettings> frontendSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _emailSender = emailSender;
            _frontendSettings = frontendSettings.Value;
        }

        public async Task<Response<string>> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !user.IsActive)
                return Response<string>.Failure("Credenciales incorrectas");

            var isValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isValid)
                return Response<string>.Failure("Credenciales incorrectas");

            var token = GenerateJwtToken(user);
            return Response<string>.Success(token, "Login exitoso");
        }

        public async Task<Response<string>> ForgotPasswordAsync(string email)
        {
            const string genericMessage = "Si el correo está registrado, recibirás un enlace de recuperación.";

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !user.IsActive)
                return Response<string>.Success(genericMessage);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);
            var resetLink = $"{_frontendSettings.BaseUrl}/reset-password?token={encodedToken}&email={Uri.EscapeDataString(email)}";

            var htmlBody = $@"
                <p>Hola {user.FullName},</p>
                <p>Recibimos una solicitud para restablecer tu contraseña en PatriSystem.</p>
                <p><a href='{resetLink}'>Haz clic aquí para restablecer tu contraseña</a></p>
                <p>Este enlace expira en 30 minutos. Si no solicitaste esto, ignora este correo.</p>";

            await _emailSender.SendAsync(email, "Recuperación de contraseña - PatriSystem", htmlBody);

            return Response<string>.Success(genericMessage);
        }

        public async Task<Response<string>> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !user.IsActive)
                return Response<string>.Failure("El enlace no es válido o ha expirado");

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return Response<string>.Failure("El enlace no es válido o ha expirado", errors);
            }

            return Response<string>.Success("Contraseña actualizada correctamente");
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("fullName", user.FullName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}