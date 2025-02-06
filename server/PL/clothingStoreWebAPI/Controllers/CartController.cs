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

        public CartController(ICartBLL cartItemBLL){
            _cartItemBLL = cartItemBLL;
        }

        /// <summary>
        /// Получает информацию о продуктах в корзине авторизованного пользователя.
        /// </summary>
        /// <returns>Информация о продуктах.</returns>
        [HttpGet]
        public async Task<ActionResult<List<CartItemDTO>>> GetCartItems(){
            try{
                var cart = await _cartItemBLL.GetCartItems();
                return Ok(cart);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Изменяет количество продукта в корзине на +1.
        /// </summary>
        /// <param name="cartId">Идентификатор продукта в корзине.</param>
        /// <returns>Информация о продукте.</returns>
        [HttpPut]
        [Route("addAmountOfCartItem")]
        public async Task<ActionResult> AddAmountOfCartItem(Guid cartId){
            try{
                await _cartItemBLL.AddAmountOfCartItem(cartId);
                return Ok();
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Изменяет количество продукта в корзине на -1.
        /// </summary>
        /// <param name="cartId">Идентификатор продукта в корзине.</param>
        /// <returns>Информация о продукте.</returns>
        [HttpPut]
        [Route("reduceAmountOfCartItem")]
        public async Task<ActionResult> ReduceAmountOfCartItem(Guid cartId){
            try{
                await _cartItemBLL.ReduceAmountOfCartItem(cartId);
                return Ok(); 
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Удаляет продукт из корзины.
        /// </summary>
        /// <param name="cartId">Идентификатор продукта в корзине.</param>
        /// <returns>Информация о продукте.</returns>
        [HttpDelete("{cartId}")]
        public async Task<ActionResult> DeleteCartItem(Guid cartId){
            try{
                await _cartItemBLL.DeleteCartItem(cartId);
                return Ok(); 
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
    }
}
