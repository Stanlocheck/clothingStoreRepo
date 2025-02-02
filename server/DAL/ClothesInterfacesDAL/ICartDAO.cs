using System;
using ClothDomain;

namespace ClothesInterfacesDAL;

public interface ICartDAO
{
    public Task<List<CartItem>> GetCartItems(Guid buyerId);
    public Task AddCartItem(Guid buyerId, Guid clothId);
    public Task AddAmount(Guid buyerId, Guid clothId);
    public Task ReduceAmount(Guid buyerId, Guid clothId);
}
