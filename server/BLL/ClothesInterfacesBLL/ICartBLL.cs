using System;
using ClothDTOs;

namespace ClothesInterfacesBLL;

public interface ICartBLL
{
    public Task<List<CartItemDTO>> GetCartItems();
    public Task AddCartItem(Guid clothId);
    public Task AddAmount(Guid clothId);
    public Task ReduceAmount(Guid clothId);
}
