using ClothDomain;

namespace ClothesInterfacesDAL;

public interface IWishlistDAO
{
    public Task<List<Wishlist>> GetAllWishlists();
    public Task<Wishlist> GetWishlist(Guid buyerId);
    public Task CreateWishlist(Guid buyerId);
    public Task<List<WishlistItem>> GetAllWishlistItems(Guid buyerId);
    public Task<WishlistItem> GetWishlistItem(Guid buyerId, Guid wishlistItemId);
    public Task AddToWishlist(Guid buyerId, Guid clothId);
    public Task FromWishlistToCart(Guid buyerId, Guid wishlistId);
    public Task DeleteWishlistItem(Guid buyerId, Guid wishlistId);
}