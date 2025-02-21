using ClothDTOs;
using ClothesInterfacesBLL;
using Microsoft.AspNetCore.Mvc;

namespace clothingStoreWebAPI.Controllers
{
    /// <summary>
    /// Контроллер для работы с авторизацией пользователей.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger){
            _logger = logger;
            _authService = authService;
        }


        /// <summary>
        /// Создает пользователя.
        /// </summary>
        /// <param name="buyer">Схема регистрации.</param>
        /// <returns>Информация о пользователе.</returns>
        [HttpPost("register")]
        public async Task<ActionResult> Register(BuyerAddDTO buyer)
        {
            try{
                await _authService.Register(buyer);
                _logger.LogInformation("Успешная регистрация пользователя");
                return Ok("Регистрация пользователя успешна.");
            }
            catch (Exception ex){
                _logger.LogWarning(ex, "Ошибка регистрации пользователя");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Вход в систему пользователя.
        /// </summary>
        /// <param name="request">Схема авторизации.</param>
        /// <returns>Информация о пользователе.</returns>
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginModel request)
        {
            try{
                await _authService.Login(request.Email, request.Password);
                _logger.LogInformation("Успешный вход пользователя");
                return Ok("Вход пользователя успешен.");
            }
            catch (Exception ex){
                _logger.LogWarning(ex, "Ошибка входа пользователя");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Выход из системы пользователя.
        /// </summary>
        /// <returns>Информация о пользователе.</returns>
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            try{
                await _authService.Logout();
                _logger.LogInformation("Успешный выход пользователя");
                return Ok("Выход пользователя успешен.");
            }
            catch (Exception ex){
                _logger.LogWarning(ex, "Ошибка выхода пользователя");
                return BadRequest(ex.Message);
            }
        }
    }
}
