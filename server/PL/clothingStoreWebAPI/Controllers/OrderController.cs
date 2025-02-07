using ClothDTOs.OrderDTOs;
using ClothesInterfacesBLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace clothingStoreWebAPI.Controllers
{
    /// <summary>
    /// Контроллер для работы с функциями заказа.
    /// </summary>
    [Authorize(Policy = "BuyerOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderBLL _orderBLL;

        public OrderController(IOrderBLL orderBLL){
            _orderBLL = orderBLL;
        }


        /// <summary>
        /// Получает информацию о всех заказах авторизованного пользователя.
        /// </summary>
        /// <returns>Информация о заказах.</returns>
        [HttpGet]
        public async Task<ActionResult<List<OrderDTO>>> GetAllOrders(){
            try{
                var order = await _orderBLL.GetAllOrders();
                return Ok(order);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Получает информацию о заказе по его идентификатору авторизованного пользователя.
        /// </summary>
        /// <param name="orderId">Идентификатор заказа.</param>
        /// <returns>Информация о заказе.</returns>
        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(Guid orderId){
            try{
                var order = await _orderBLL.GetOrder(orderId);
                return Ok(order);
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Создает заказ.
        /// </summary>
        /// <returns>Информация о заказе.</returns>
        [HttpPost]
        public async Task<ActionResult> CreateOrder(){
            try{
                await _orderBLL.CreateOrder();
                return Ok();
            }
            catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }
    }
}
