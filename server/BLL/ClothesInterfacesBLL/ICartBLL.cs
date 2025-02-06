using ClothDTOs;

namespace ClothesInterfacesBLL;

public interface ICartBLL
{
    public Task<CartDTO> GetCart();
    public Task AddToCart(Guid clothId);
    public Task AddAmountOfCartItem(Guid cartId);
    public Task ReduceAmountOfCartItem(Guid cartId);
    public Task DeleteCartItem(Guid cartId);
    public Task SelectCartItem(Guid cartId);
}
