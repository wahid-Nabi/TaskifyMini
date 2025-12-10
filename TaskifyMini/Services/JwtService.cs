using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskifyMini.Data;
using TaskifyMini.Helpers.ConfigurationSetting;
using TaskifyMini.Models.DTOs;
using TaskifyMini.Models.Entities;

namespace TaskifyMini.Services
{
    public class JwtService : IJwtService
    {
        private readonly Jwt _jwtSettings;
        private readonly TaskifyContext _context;
        public JwtService(IOptions<Jwt> options, TaskifyContext context)
        {
            _jwtSettings = options.Value;
            _context = context;
        }

        public string GenerateAccessToken(User user)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.RoleName),
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                SigningCredentials = credentials,
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }          
        }
        public (string accessToken, string refreshToken) GenerateTokens(User user)
        {
            var accessToken = GenerateAccessToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

            _context.Users.Update(user);
            _context.SaveChanges();

            return (accessToken, refreshToken);
        }
        public async Task<(bool success, string? newAccessToken, string? newRefreshToken)> RefreshTokenAsync(RefreshRequestDto model)
        {
            // Validate the expired access token and get the principal
            var principal = GetPrincipalFromExpiredToken(model.AccessToken);
            if (principal == null) return (false, null, null);
            // Get the user email from the principal
            var userEmail = principal.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _context.Users
                        .Include(r => r.Role)
                        .FirstOrDefaultAsync(u => u.Email == userEmail);
            // Validate the refresh token
            if (user == null ||
                user.RefreshToken != model.RefreshToken ||
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return (false, null, null);
            }
            // Generate new tokens
            var newAccessToken = GenerateAccessToken(user);
            var newRefreshToken = GenerateRefreshToken();
            // Update the user with the new refresh token
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return (true, newAccessToken, newRefreshToken);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidateLifetime = false // ignore expiration
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

    }
}
