using ClothDomain;

namespace ClothesInterfacesDAL;

public interface ICartDAO
{
    public Task<List<Cart>> GetAllCarts();
    public Task<Cart> GetCart(Guid buyerId);
    public Task CreateCart(Guid buyerId);
    public Task<CartItem> GetCartItem(Guid buyerId, Guid cartItemId);
    public Task AddToCart(Guid buyerId, Guid clothId);
    public Task AddAmountOfCartItem(Guid buyerId, Guid cartId);
    public Task ReduceAmountOfCartItem(Guid buyerId, Guid cartId);
    public Task DeleteCartItem(Guid buyerId, Guid cartId);
    public Task SelectCartItem(Guid buyerId, Guid cartId);
}
