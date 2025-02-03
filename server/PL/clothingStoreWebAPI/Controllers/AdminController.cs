using Microsoft.AspNetCore.Http;
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

        public AdminController(IAdminsBLL adminsBLL){
            _adminBLL = adminsBLL;
        }
        

        /// <summary>
        /// Изменяет информацию авторизованного модератора.
        /// </summary>
        /// <param name="updtAdmin">Схема модератора.</param>
        /// <returns>Информация о модераторе.</returns>
        [HttpPut]
        public async Task<ActionResult> UpdateAdmin(AdminAddDTO updtAdmin){
            try{
                await _adminBLL.UpdateAdmin(updtAdmin);
                return Ok(updtAdmin);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
    }
}
