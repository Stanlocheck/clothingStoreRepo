using ClothDomain;

namespace ClothesInterfacesDAL;

public interface ICartDAO
{
    public Task<List<CartItem>> GetCartItems(Guid buyerId);
    public Task AddAmountOfCartItem(Guid buyerId, Guid cartId);
    public Task ReduceAmountOfCartItem(Guid buyerId, Guid cartId);
    public Task DeleteCartItem(Guid buyerId, Guid cartId);
    public Task SelectCartItem(Guid buyerId, Guid cartId);
}
