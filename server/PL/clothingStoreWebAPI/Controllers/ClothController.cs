using Microsoft.AspNetCore.Mvc;
using ClothesInterfacesBLL;
using ClothDTOs;
using Microsoft.AspNetCore.Authorization;
using ClothDTOs.WishlistDTOs;

namespace clothingStoreWebAPI.Controllers
{
    /// <summary>
    /// Контроллер для работы с функциями продуктов.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ClothController : ControllerBase
    {
        private IClothBLL _clothBLL;
        private ICartBLL _cartItemBLL;
        private IWishlistBLL _wishlistItemBLL;

        public ClothController(IClothBLL clothBLL, ICartBLL cartItemBLL, IWishlistBLL wishlistBLL) {
            _clothBLL = clothBLL;
            _cartItemBLL = cartItemBLL;
            _wishlistItemBLL = wishlistBLL;
        }


        /// <summary>
        /// Получает информацию о всех продуктах.
        /// </summary>
        /// <returns>Информация о продуктах.</returns>
        [HttpGet]
        public async Task<ActionResult<List<ClothDTO>>> GetAll(){
            try{
                var cloth = await _clothBLL.GetAll();
                return Ok(cloth);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Получает информацию о продукте по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор продукта.</param>
        /// <returns>Информация о продукте.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ClothDTO>> GetById(Guid id){
            try{
                var cloth = await _clothBLL.GetById(id);
                return Ok(cloth);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Создает продукт.
        /// </summary>
        /// <returns>Информация о продукте.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult> AddCloth(ClothAddDTO addCloth){
            try{
                await _clothBLL.AddCloth(addCloth);
                return Ok(addCloth);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Изменяет информацию о продукте по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор продукта.</param>
        /// <param name="updtCloth">Схема продукта.</param>
        /// <returns>Информация о продукте.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCloth(ClothAddDTO updtCloth, Guid id){
            try{
                await _clothBLL.UpdateCloth(updtCloth, id);
                return Ok(updtCloth);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Удаляет продукт по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор продукта.</param>
        /// <returns>Информация о продукте.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCloth(Guid id){
            try{
                await _clothBLL.DeleteCloth(id);
                return Ok();
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Получает информацию о мужских продуктах.
        /// </summary>
        /// <returns>Информация о продуктах.</returns>
        [HttpGet]
        [Route("mensClothing")]
        public async Task<ActionResult<List<ClothDTO>>> GetMensClothing(){
            try{
                var cloth = await _clothBLL.GetMensClothing();
                return Ok(cloth);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Получает информацию о женских продуктах.
        /// </summary>
        /// <returns>Информация о продуктах.</returns>
        [HttpGet]
        [Route("womensClothing")]
        public async Task<ActionResult<List<ClothDTO>>> GetWomensClothing(){
            try{
                var cloth = await _clothBLL.GetWomensClothing();
                return Ok(cloth);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Добавляет продукт в корзину авторизованного пользователя.
        /// </summary>
        /// <param name="clothId">Идентификатор продукта.</param>
        /// <returns>Информация о продукте.</returns>
        [Authorize(Policy = "BuyerOnly")]
        [HttpPost]
        [Route("addToCart")]
        public async Task<ActionResult> AddToCart(Guid clothId){
            try{
                await _cartItemBLL.AddToCart(clothId);
                return Ok();
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Получает информацию о продуктах в корзине авторизованного пользователя.
        /// </summary>
        /// <returns>Информация о продуктах.</returns>
        [Authorize(Policy = "BuyerOnly")]
        [HttpGet]
        [Route("cart")]
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
        [Authorize(Policy = "BuyerOnly")]
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
        [Authorize(Policy = "BuyerOnly")]
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
        [Authorize(Policy = "BuyerOnly")]
        [HttpDelete]
        [Route("deleteCartItem")]
        public async Task<ActionResult> DeleteCartItem(Guid cartId){
            try{
                await _cartItemBLL.DeleteCartItem(cartId);
                return Ok(); 
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Добавляет продукт в вишлист авторизованного пользователя.
        /// </summary>
        /// <param name="clothId">Идентификатор продукта.</param>
        /// <returns>Информация о продукте.</returns>
        [Authorize(Policy = "BuyerOnly")]
        [HttpPost]
        [Route("addToWishlist")]
        public async Task<ActionResult> AddToWishlist(Guid clothId){
            try{
                await _wishlistItemBLL.AddToWishlist(clothId);
                return Ok();
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Получает информацию о продуктах в вишлисте авторизованного пользователя.
        /// </summary>
        /// <returns>Информация о продуктах.</returns>
        [Authorize(Policy = "BuyerOnly")]
        [HttpGet]
        [Route("wishlist")]
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
        /// Добавляет продукт из вишлиста в корзину.
        /// </summary>
        /// <param name="wishlistId">Идентификатор продукта в вишлисте.</param>
        /// <returns>Информация о продукте.</returns>
        [Authorize(Policy = "BuyerOnly")]
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
        [Authorize(Policy = "BuyerOnly")]
        [HttpDelete]
        [Route("deleteWishlistItem")]
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
