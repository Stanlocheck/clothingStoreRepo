using System;
using ClothDomain;

namespace ClothesInterfacesDAL;

public interface ICartDAO
{
    public Task AddCartItem(Guid buyerId, Guid clothId, int amount);
}
