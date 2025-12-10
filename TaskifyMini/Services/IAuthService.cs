using TaskifyMini.Helpers;
using TaskifyMini.Models.DTOs;

namespace TaskifyMini.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginEntityDto model);
        Task<ApiResponse<RegisterResponseDto?>> RegisterAsync(RegisterRequestDto model); 
        Task<ApiResponse<string>> LogoutAsync();
    }
}
