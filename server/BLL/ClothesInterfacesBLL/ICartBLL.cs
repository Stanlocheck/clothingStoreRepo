using System;
using ClothDTOs;

namespace ClothesInterfacesBLL;

public interface ICartBLL
{
    public Task<List<CartItemDTO>> GetCartItems();
    public Task AddToCart(Guid clothId);
    public Task AddAmountOfCartItem(Guid cartId);
    public Task ReduceAmountOfCartItem(Guid cartId);
    public Task DeleteCartItem(Guid cartId);
}
