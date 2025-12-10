using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskifyMini.Models.DTOs;
using TaskifyMini.Services;

namespace TaskifyMini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IAuthService _authService;
        public AuthController(IJwtService jwtService, IAuthService authService)
        {
            _jwtService = jwtService;
            _authService = authService;
        }
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshRequestDto requestDto)
        {
            var result = await _jwtService.RefreshTokenAsync(requestDto);
            if (!result.success)
            {
                return BadRequest();
            }

            return Ok(new RefreshRequestDto()
            {
                AccessToken = result.newAccessToken!,
                RefreshToken = result.newRefreshToken!
            });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginEntityDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            return Ok(result);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
        {       
            var result = await _authService.RegisterAsync(registerRequestDto);
            if(!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _authService.LogoutAsync();
            if(!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
