using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClothesInterfacesBLL;

namespace clothingStoreWebAPI.Controllers
{
    /// <summary>
    /// Контроллер для работы с авторизацией модераторов.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAdminController : ControllerBase
    {
        private readonly IAuthAdminService _authAdminService;

        public AuthAdminController(IAuthAdminService authAdminService){
            _authAdminService = authAdminService;
        }


        /// <summary>
        /// Создает модератора.
        /// </summary>
        /// <param name="request">Схема регистрации.</param>
        /// <returns>Информация о модераторе.</returns>
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


        /// <summary>
        /// Вход в систему модератора.
        /// </summary>
        /// <param name="request">Схема авторизации.</param>
        /// <returns>Информация о модераторе.</returns>
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


        /// <summary>
        /// Выход из системы модератора.
        /// </summary>
        /// <returns>Информация о модераторе.</returns>
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
