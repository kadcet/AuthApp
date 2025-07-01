using AuthApp.Bll.Services;
using AuthApp.Model.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AuthApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var success = await _userService.RegisterAsync(dto.UserName, dto.Password);
            if (!success)
                return BadRequest("Kullanıcı zaten kayıtlı.");

            return Ok("Kayıt başarılı.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var success = await _userService.LoginAsync(dto.UserName, dto.Password);
            if (!success)
                return Unauthorized("Kullanıcı adı veya şifre yanlış.");

            return Ok("Giriş başarılı.");
        }
    }
}
