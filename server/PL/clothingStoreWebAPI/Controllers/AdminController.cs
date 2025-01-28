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
        /// Изменяет информацию о модераторе по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор модератора.</param>
        /// <param name="updtAdmin">Схема модератора.</param>
        /// <returns>Информация о модераторе.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAdmin(AdminAddDTO updtAdmin, Guid id){
            try{
                await _adminBLL.UpdateAdmin(updtAdmin, id);
                return Ok(updtAdmin);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
    }
}
