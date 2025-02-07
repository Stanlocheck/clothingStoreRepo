using System;
using ClothDTOs.OrderDTOs;

namespace ClothesInterfacesBLL;

public interface IOrderBLL
{
    public Task<List<OrderDTO>> GetAllOrders();
    public Task<OrderDTO> GetOrder(Guid orderId);
    public Task CreateOrder();
    public Task SelectOrderStatus(Guid orderId, string status);
}
