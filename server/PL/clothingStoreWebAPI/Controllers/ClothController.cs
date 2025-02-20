using Microsoft.AspNetCore.Mvc;
using ClothesInterfacesBLL;
using ClothDTOs;
using Microsoft.AspNetCore.Authorization;
using clothingStoreWebAPI.Models;

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
        private ILogger<ClothController> _logger;

        public ClothController(IClothBLL clothBLL, ILogger<ClothController> logger) {
            _logger = logger;
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
                _logger.LogWarning(ex, "Ошибка получении информации о продуктах");
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
                _logger.LogWarning(ex, "Ошибка получении информации о продукте");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Создает продукт.
        /// </summary>
        /// <param name="addCloth">Информация о продукте.</param>
        /// <param name="files">Загружаемые изображения.</param>
        /// <returns>Информация о продукте.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<ActionResult> AddCloth([FromForm] ClothAddDTO addCloth, [FromForm] UploadImageModel files){
            try{
                await _clothBLL.AddCloth(addCloth, files.Files);
                return Ok(addCloth);
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка создании продукта");
                return BadRequest(ex.Message);
            }
        }


        /*/// <summary>
        /// Изменяет информацию о продукте по его идентификатору.
        /// </summary>
        /// <param name="updtCloth">Схема продукта.</param>
        /// <returns>Информация о продукте.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCloth(ClothAddDTO updtCloth){
            try{
                await _clothBLL.UpdateCloth(updtCloth);
                return Ok(updtCloth);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }*/


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
                _logger.LogWarning(ex, "Ошибка удаления продукта");
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
                _logger.LogWarning(ex, "Ошибка получения информации о женских продуктах");
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
                _logger.LogWarning(ex, "Ошибка получения информации о женских продуктах");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Добавляет изображения в существующий продукт.
        /// </summary>
        /// <param name="clothId">Идентификатор продукта.</param>
        /// <param name="files">Загружаемые изображения.</param>
        /// <returns>Информация о продукте.</returns>
        [HttpPost]
        [Route("addImage")]
        public async Task<ActionResult> AddImage(Guid clothId, [FromForm] UploadImageModel files){
            try{
                await _clothBLL.AddImage(clothId, files.Files);
                return Ok();
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка добавления изображения");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Удаляет изображения продукта по его идентификатору.
        /// </summary>
        /// <param name="clothId">Идентификатор продукта.</param>
        /// <param name="files">Удаляемые изображения.</param>
        /// <returns>Информация о продукте.</returns>
        [HttpDelete]
        [Route("deleteImage")]
        public async Task<ActionResult> DeleteImage(Guid clothId, [FromForm] IEnumerable<Guid> files){
            try{
                await _clothBLL.DeleteImage(clothId, files);
                return Ok();
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка удаления изображения");
                return BadRequest(ex.Message);
            }
        }
    }
}
