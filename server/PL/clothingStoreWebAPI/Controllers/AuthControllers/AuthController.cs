using ClothesInterfacesBLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace clothingStoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService){
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterModel request)
        {
            try
            {
                await _authService.Register(request.BuyerInfo);
                return Ok("Регистрация пользователя успешна.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
                [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginModel request)
        {
            try
            {
                await _authService.Login(request.Email, request.Password);
                return Ok("Вход пользователя успешен.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                await _authService.Logout();
                return Ok("Выход пользователя успешен.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
