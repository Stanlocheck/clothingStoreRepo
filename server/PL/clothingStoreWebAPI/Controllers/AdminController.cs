using Microsoft.AspNetCore.Mvc;
using ClothesInterfacesBLL;
using ClothDTOs;
using Microsoft.AspNetCore.Authorization;

namespace clothingStoreWebAPI.Controllers
{
    /// <summary>
    /// Контроллер для работы с функциями модераторов.
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IAdminsBLL _adminBLL;
        private ILogger<AdminController> _logger;

        public AdminController(IAdminsBLL adminsBLL, ILogger<AdminController> logger){
            _logger = logger;
            _adminBLL = adminsBLL;
        }
        

        /// <summary>
        /// Изменяет информацию авторизованного модератора.
        /// </summary>
        /// <param name="updtAdmin">Схема модератора.</param>
        /// <returns>Информация о модераторе.</returns>
        [HttpPut]
        public async Task<ActionResult> UpdateAdmin(AdminUpdateDTO updtAdmin){
            try{
                await _adminBLL.UpdateAdmin(updtAdmin);
                _logger.LogInformation("Успешное изменение информации");
                return Ok(updtAdmin);
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка изменения информации");
                return BadRequest(ex.Message);
            }
        }
    }
}
