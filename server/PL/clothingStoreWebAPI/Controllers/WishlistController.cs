using Microsoft.AspNetCore.Mvc;
using ClothesInterfacesBLL;
using Microsoft.AspNetCore.Authorization;
using ClothDTOs.WishlistDTOs;

namespace clothingStoreWebAPI.Controllers
{
    /// <summary>
    /// Контроллер для работы с функциями вишлиста.
    /// </summary>
    [Authorize(Policy = "BuyerOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private IWishlistBLL _wishlistItemBLL;

        public WishlistController(IWishlistBLL wishlistItemBLL){
            _wishlistItemBLL = wishlistItemBLL;
        }


        /// <summary>
        /// Получает информацию о продуктах в вишлисте авторизованного пользователя.
        /// </summary>
        /// <returns>Информация о продуктах.</returns>
        [HttpGet]
        public async Task<ActionResult<List<WishlistItemDTO>>> GetWishlistItems(){
            try{
                var wishlist = await _wishlistItemBLL.GetWishlistItems();
                return Ok(wishlist);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        
        /// <summary>
        /// Добавляет продукт в корзину из вишлиста.
        /// </summary>
        /// <param name="wishlistId">Идентификатор продукта в вишлисте.</param>
        /// <returns>Информация о продукте.</returns>
        [HttpPost]
        [Route("fromWishlistToCart")]
        public async Task<ActionResult> FromWishlistToCart(Guid wishlistId){
            try{
                await _wishlistItemBLL.FromWishlistToCart(wishlistId);
                return Ok();
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Удаляет продукт из вишлиста.
        /// </summary>
        /// <param name="wishlistId">Идентификатор продукта в вишлисте.</param>
        /// <returns>Информация о продукте.</returns>
        [HttpDelete("{wishlistId}")]
        public async Task<ActionResult> DeleteWishlistItem(Guid wishlistId){
            try{
                await _wishlistItemBLL.DeleteWishlistItem(wishlistId);
                return Ok(); 
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
    }
}
