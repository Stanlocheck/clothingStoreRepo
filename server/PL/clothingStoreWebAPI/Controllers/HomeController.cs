using Microsoft.AspNetCore.Mvc;
using ClothesInterfacesBLL;
using ClothDTOs;
using Microsoft.AspNetCore.Authorization;

namespace clothingStoreWebAPI.Controllers
{
    /// <summary>
    /// Контроллер для работы с функциями продуктов.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IClothBLL _clothBLL;

        public HomeController(IClothBLL clothBLL) {
            _clothBLL = clothBLL;
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
    }
}
