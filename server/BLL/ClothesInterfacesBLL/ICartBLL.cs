using System;
using ClothDTOs;

namespace ClothesInterfacesBLL;

public interface ICartBLL
{
    public Task<List<CartItemDTO>> GetCartItems(Guid buyerId);
    public Task AddCartItem(Guid buyerId, Guid clothId);
}
