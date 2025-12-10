using TaskifyMini.Models.DTOs;
using TaskifyMini.Models.Entities;

namespace TaskifyMini.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        (string accessToken, string refreshToken) GenerateTokens(User user);
        Task<(bool success, string? newAccessToken, string? newRefreshToken)> RefreshTokenAsync(RefreshRequestDto model);
    }
}
