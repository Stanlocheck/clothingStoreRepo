using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClothesInterfacesBLL;

namespace clothingStoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAdminController : ControllerBase
    {
        private readonly IAuthAdminService _authAdminService;

        public AuthAdminController(IAuthAdminService authAdminService){
            _authAdminService = authAdminService;
        }

        [HttpPost("registerAdmin")]
        public async Task<ActionResult> Register([FromBody] RegisterAdminModel request)
        {
            try
            {
                await _authAdminService.Register(request.AdminInfo);
                return Ok("Регистрация пользователя успешна.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("loginAdmin")]
        public async Task<ActionResult> Login([FromBody] LoginModel request)
        {
            try
            {
                await _authAdminService.Login(request.Email, request.Password);
                return Ok("Вход пользователя успешен.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("logoutAdmin")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                await _authAdminService.Logout();
                return Ok("Выход пользователя успешен.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
