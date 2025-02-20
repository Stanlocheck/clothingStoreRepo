using ClothDTOs;
using ClothesInterfacesBLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace clothingStoreWebAPI.Controllers
{
    /// <summary>
    /// Контроллер для работы с функциями покупателей.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BuyerController : ControllerBase
    {
        private IBuyersBLL _buyerBLL;
        private ILogger<AdminController> _logger;

        public BuyerController(IBuyersBLL buyersBLL, ILogger<AdminController> logger){
            _logger = logger;
            _buyerBLL = buyersBLL;
        }


        /// <summary>
        /// Получает информацию о всех пользователях.
        /// </summary>
        /// <returns>Информация о пользователях.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<ActionResult<List<BuyerDTO>>> GetAll(){
            try{
                var buyer = await _buyerBLL.GetAll();
                return Ok(buyer);
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка получения информации о пользователях");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Получает информацию о пользователе по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <returns>Информация о пользователе.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}")]
        public async Task<ActionResult<BuyerDTO>> GetById(Guid id){
            try{
                var buyer = await _buyerBLL.GetById(id);
                return Ok(buyer);
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка получения информации о пользователе");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Изменяет информацию авторизованного пользователя.
        /// </summary>
        /// <param name="updtBuyer">Схема пользователя.</param>
        /// <returns>Информация о пользователе.</returns>
        [Authorize(Policy = "BuyerOnly")]
        [HttpPut]
        public async Task<ActionResult> UpdateBuyer(BuyerUpdateDTO updtBuyer){
            try{
                await _buyerBLL.UpdateBuyer(updtBuyer);
                return Ok(updtBuyer);
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка изменения информации");
                return BadRequest(ex.Message);
            }
        }
    }
}
