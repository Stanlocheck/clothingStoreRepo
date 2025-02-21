using ClothDTOs.OrderDTOs;
using ClothesInterfacesBLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace clothingStoreWebAPI.Controllers
{
    /// <summary>
    /// Контроллер для работы с функциями заказа.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderBLL _orderBLL;
        private ILogger<ClothController> _logger;

        public OrderController(IOrderBLL orderBLL, ILogger<ClothController> logger){
            _logger = logger;
            _orderBLL = orderBLL;
        }


        /// <summary>
        /// Получает информацию о всех заказах авторизованного пользователя.
        /// </summary>
        /// <returns>Информация о заказах.</returns>
        [Authorize(Policy = "BuyerOnly")]
        [HttpGet]
        public async Task<ActionResult<List<OrderDTO>>> GetAllOrders(){
            try{
                var order = await _orderBLL.GetAllOrders();
                _logger.LogInformation("Успешное получение информации о заказах");
                return Ok(order);
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка получения информации о заказах");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Получает информацию о заказе по его идентификатору авторизованного пользователя.
        /// </summary>
        /// <param name="orderId">Идентификатор заказа.</param>
        /// <returns>Информация о заказе.</returns>
        [Authorize(Policy = "BuyerOnly")]
        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(Guid orderId){
            try{
                var order = await _orderBLL.GetOrder(orderId);
                _logger.LogInformation("Успешное получение информации о заказе");
                return Ok(order);
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка получения информации о заказе");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Создает заказ.
        /// </summary>
        /// <returns>Информация о заказе.</returns>
        [Authorize(Policy = "BuyerOnly")]
        [HttpPost]
        public async Task<ActionResult> CreateOrder(){
            try{
                await _orderBLL.CreateOrder();
                _logger.LogInformation("Успешное создание заказа");
                return Ok();
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка создания заказа");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Изменяет статус заказа на выбранный.
        /// </summary>
        /// <param name="orderId">Идентификатор заказа.</param>
        /// <param name="status">Статус заказа.</param>
        /// <returns>Информация о заказе.</returns>
        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public async Task<ActionResult> SelectOrderStatus(Guid orderId, string status){
            try{
                await _orderBLL.SelectOrderStatus(orderId, status);
                _logger.LogInformation("Успешное изменение статуса заказа");
                return Ok();
            }
            catch(Exception ex){
                _logger.LogWarning(ex, "Ошибка изменения статуса заказа");
                return BadRequest(ex.Message);
            }
        }
    }
}
