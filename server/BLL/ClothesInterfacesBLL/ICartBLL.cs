using System;
using ClothDTOs;

namespace ClothesInterfacesBLL;

public interface ICartBLL
{
    public Task AddCartItem(Guid buyerId, Guid clothId, int amount);
}
