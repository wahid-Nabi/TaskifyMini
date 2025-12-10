using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskifyMini.Helpers;
using TaskifyMini.Helpers.Tokens;
using TaskifyMini.Models.DTOs;
using TaskifyMini.Models.Entities;
using TaskifyMini.Models.Enums;
using TaskifyMini.Repositories.Interfaces;

namespace TaskifyMini.Services
{

    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IUserRepository userRepository, IJwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            this.userRepository = userRepository;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginEntityDto model)
        {
            var user = await userRepository.GetUserByEmailAsync(model.Email);
            if (user == null)
                return new ApiResponse<LoginResponseDto>
                {
                    IsSuccess = false,
                    Message = "Invalid email",
                    Data = null
                };

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return new ApiResponse<LoginResponseDto>
                {
                    IsSuccess = false,
                    Message = "Invalid password",
                    Data = null
                };
            }
            // Generate tokens (access and refresh)
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await userRepository.UpdateUserAsync(user);
            var loginResponse = new LoginResponseDto()
            {
                UserId = user.Id,
                Email = model.Email,
                AccessToken = accessToken,
                RefreshToken = refreshToken,

            };
            return new ApiResponse<LoginResponseDto>
            {
                IsSuccess = true,
                Data = loginResponse,
                Message = "User logged in successfully."
            };

        }

        public async Task<ApiResponse<string>> LogoutAsync()
        {

            var userId = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return new ApiResponse<string>
            {
                IsSuccess = false,
                Message = "User is Unauthorized."
            };
            var user = await userRepository.GetUserByIdAsync(int.Parse(userId));

            if (user == null) return new ApiResponse<string>
            {
                IsSuccess = false,
                Message = "User not found."
            };
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            var updatedUser = await userRepository.UpdateUserAsync(user);
            if (updatedUser == null) return new ApiResponse<string>
            {
                IsSuccess = false,
                Message = "Logout failed."
            };
            return new ApiResponse<string>
            {
                IsSuccess = true,
                Message = "User logged out successfully."
            };
        }

        public async Task<ApiResponse<RegisterResponseDto?>> RegisterAsync(RegisterRequestDto model)
        {
            var existingUser = await userRepository.GetUserByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return new ApiResponse<RegisterResponseDto?>
                {
                    IsSuccess = false,
                    Message = "User with this email already exists.",
                    Data = null
                };
            }
            var passwordHasher = new PasswordHasher<User>();
            var newUser = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                Phone = model.Phone,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false,
                RoleId = (int)UserRoles.User
            };
            newUser.PasswordHash = passwordHasher.HashPassword(newUser, model.Password);
            var registeredUser = await userRepository.AddUserAsync(newUser);
            if (registeredUser == null)
                return new ApiResponse<RegisterResponseDto?>
                {
                    IsSuccess = false,
                    Message = "User registration failed.",
                    Data = null
                };
            var responseDto = new RegisterResponseDto
            {
                UserId = registeredUser.Id,
                UserName = registeredUser.UserName,
                Email = registeredUser.Email,
                Phone = registeredUser.Phone// Assuming phone is not part of registration
            };

            return new ApiResponse<RegisterResponseDto?>
            {
                IsSuccess = true,
                Message = "User registered successfully.",
                Data = responseDto
            };
        }

    }
}
