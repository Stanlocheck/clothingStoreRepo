using ClothesInterfacesBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ClothDTOs;

namespace clothingStoreWebAPI.Controllers
{
    /// <summary>
    /// Контроллер для работы с функциями корзины.
    /// </summary>
    [Authorize(Policy = "BuyerOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private ICartBLL _cartItemBLL;
        private ILogger<CartController> _logger;

        public CartController(ICartBLL cartItemBLL, ILogger<CartController> logger){
            _logger = logger;
            _cartItemBLL = cartItemBLL;
        }


        /// <summary>
        /// Получает информацию о продуктах в корзине авторизованного пользователя.
        /// </summary>
        /// <returns>Информация о продуктах.</returns>
        [HttpGet]
        public async Task<ActionResult<CartDTO>> GetCart(){
            try{
                var cart = await _cartItemBLL.GetCart();
                _logger.LogInformation("Успешное получение информации о продуктах");
                return Ok(cart);
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка получения информации о продуктах");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Добавляет продукт в корзину авторизованного пользователя.
        /// </summary>
        /// <param name="clothId">Идентификатор продукта.</param>
        /// <returns>Информация о продукте.</returns>
        [HttpPost]
        public async Task<ActionResult> AddToCart(Guid clothId){
            try{
                await _cartItemBLL.AddToCart(clothId);
                _logger.LogInformation("Успешное добавление продукта");
                return Ok();
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка добавления продукта");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Изменяет количество продукта в корзине на +1.
        /// </summary>
        /// <param name="cartItemId">Идентификатор продукта в корзине.</param>
        /// <returns>Информация о продукте.</returns>
        [HttpPut]
        [Route("addAmountOfCartItem")]
        public async Task<ActionResult> AddAmountOfCartItem(Guid cartItemId){
            try{
                await _cartItemBLL.AddAmountOfCartItem(cartItemId);
                _logger.LogInformation("Успешное изменение количества продукта");
                return Ok();
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка изменения количества продукта");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Изменяет количество продукта в корзине на -1.
        /// </summary>
        /// <param name="cartItemId">Идентификатор продукта в корзине.</param>
        /// <returns>Информация о продукте.</returns>
        [HttpPut]
        [Route("reduceAmountOfCartItem")]
        public async Task<ActionResult> ReduceAmountOfCartItem(Guid cartItemId){
            try{
                await _cartItemBLL.ReduceAmountOfCartItem(cartItemId);
                _logger.LogInformation("Успешное изменение количества продукта");
                return Ok(); 
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка изменения количества продукта");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Удаляет продукт из корзины.
        /// </summary>
        /// <param name="cartItemId">Идентификатор продукта в корзине.</param>
        /// <returns>Информация о продукте.</returns>
        [HttpDelete("{cartItemId}")]
        public async Task<ActionResult> DeleteCartItem(Guid cartItemId){
            try{
                await _cartItemBLL.DeleteCartItem(cartItemId);
                _logger.LogInformation("Успешное удаление продукта");
                return Ok(); 
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка удаления продукта");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Меняет статус продукта в корзине на выбран/не выбран для заказа.
        /// </summary>
        /// <param name="cartItemId">Идентификатор продукта в корзине.</param>
        /// <returns>Информация о продукте.</returns>
        [HttpPut("{cartItemId}")]
        public async Task<ActionResult> SelectCartItem(Guid cartItemId){
            try{
                await _cartItemBLL.SelectCartItem(cartItemId);
                _logger.LogInformation("Успешное изменение статуса продукта");
                return Ok();
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка изменения статуса продукта");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Получает информацию о продукте в корзине.
        /// </summary>
        /// <param name="cartItemId">Идентификатор продукта в корзине.</param>
        /// <returns>Информация о продукте.</returns>
        [HttpGet]
        [Route("getCartItem")]
        public async Task<ActionResult> GetCartItem(Guid cartItemId){
            try{
                var cloth = await _cartItemBLL.GetCartItem(cartItemId);
                return RedirectToAction("GetById", "Cloth", new { id = cloth.ClothId});
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка получения информации о продукте");
                return BadRequest(ex.Message);
            }
        }
    }
}
