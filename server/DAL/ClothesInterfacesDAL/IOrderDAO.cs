using ClothDomain;

namespace ClothesInterfacesDAL;

public interface IOrderDAO
{
    public Task<List<Order>> GetAllOrders(Guid buyerId);
    public Task<Order> GetOrder(Guid buyerId, Guid orderId);
    public Task CreateOrder(Guid buyerId);
}
