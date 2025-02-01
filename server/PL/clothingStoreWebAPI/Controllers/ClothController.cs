using Microsoft.AspNetCore.Mvc;
using ClothesInterfacesBLL;
using ClothDTOs;
using Microsoft.AspNetCore.Authorization;
using ClothDomain;

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

        public ClothController(IClothBLL clothBLL, ICartBLL cartItemBLL) {
            _clothBLL = clothBLL;
            _cartItemBLL = cartItemBLL;
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


        /*/// <summary>
        /// Получает информацию отфильтрованную по полу.
        /// </summary>
        /// <param name="gender">Пол.</param>
        /// <returns>Информация о продуктах.</returns>
        [HttpGet]
        [Route("filterBySex")]
        public async Task<ActionResult<List<ClothDTO>>> FilterBySex(Gender gender){
            try{
                var cloth = await _clothBLL.FilterBySex(gender);
                return Ok(cloth);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }*/


        /// <summary>
        /// Добавляет продукт в корзину.
        /// </summary>
        /// <param name="buyerId">Идентификатор покупателя.</param>
        /// <param name="clothId">Идентификатор продукта.</param>
        /// <returns>Информация о продукте.</returns>
        [Authorize(Policy = "BuyerOnly")]
        [HttpPost]
        [Route("AddToCart")]
        public async Task<ActionResult> AddCartItem(Guid buyerId, Guid clothId){
            try{
                await _cartItemBLL.AddCartItem(buyerId, clothId);
                return Ok();
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Получает информацию о продуктах в корзине по идентификатору покупателя.
        /// </summary>
        /// <param name="buyerId">Идентификатор покупателя.</param>
        /// <returns>Информация о продуктах.</returns>
        [Authorize(Policy = "BuyerOnly")]
        [HttpGet]
        [Route("cart")]
        public async Task<ActionResult<List<CartItemDTO>>> GetCartItems(Guid buyerId){
            try{
                var cart = await _cartItemBLL.GetCartItems(buyerId);
                return Ok(cart); 
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
    }
}
