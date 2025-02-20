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
        private ILogger<ClothController> _logger;

        public WishlistController(IWishlistBLL wishlistItemBLL, ILogger<ClothController> logger){
            _logger = logger;
            _wishlistItemBLL = wishlistItemBLL;
        }


        /// <summary>
        /// Получает информацию о продуктах в вишлисте авторизованного пользователя.
        /// </summary>
        /// <returns>Информация о продуктах.</returns>
        [HttpGet]
        public async Task<ActionResult<List<WishlistItemDTO>>> GetAllWishlistItems(){
            try{
                var wishlist = await _wishlistItemBLL.GetAllWishlistItems();
                return Ok(wishlist);
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка получении информации о продуктах");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Добавляет продукт в вишлист авторизованного пользователя.
        /// </summary>
        /// <param name="clothId">Идентификатор продукта.</param>
        /// <returns>Информация о продукте.</returns>
        [HttpPost]
        public async Task<ActionResult> AddToWishlist(Guid clothId){
            try{
                await _wishlistItemBLL.AddToWishlist(clothId);
                return Ok();
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка добавления продукта");
                return BadRequest(ex.Message);
            }
        }

        
        /// <summary>
        /// Добавляет продукт в корзину из вишлиста.
        /// </summary>
        /// <param name="wishlistItemId">Идентификатор продукта в вишлисте.</param>
        /// <returns>Информация о продукте.</returns>
        [HttpPost]
        [Route("fromWishlistToCart")]
        public async Task<ActionResult> FromWishlistToCart(Guid wishlistItemId){
            try{
                await _wishlistItemBLL.FromWishlistToCart(wishlistItemId);
                return Ok();
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка добавления продукта");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Удаляет продукт из вишлиста.
        /// </summary>
        /// <param name="wishlistItemId">Идентификатор продукта в вишлисте.</param>
        /// <returns>Информация о продукте.</returns>
        [HttpDelete("{wishlistItemId}")]
        public async Task<ActionResult> DeleteWishlistItem(Guid wishlistItemId){
            try{
                await _wishlistItemBLL.DeleteWishlistItem(wishlistItemId);
                return Ok(); 
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка удаления продукта");
                return BadRequest(ex.Message);
            }
        }
    }
}
