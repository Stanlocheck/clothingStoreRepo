using ClothDTOs.WishlistDTOs;

namespace ClothesInterfacesBLL;

public interface IWishlistBLL
{
    public Task<List<WishlistItemDTO>> GetWishlistItems();
    public Task FromWishlistToCart(Guid wishlistId);
    public Task DeleteWishlistItem(Guid wishlistId);
}
