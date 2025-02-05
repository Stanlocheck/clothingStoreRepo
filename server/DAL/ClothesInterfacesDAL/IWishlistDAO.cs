using System;
using ClothDomain;

namespace ClothesInterfacesDAL;

public interface IWishlistDAO
{
    public Task<List<WishlistItem>> GetWishlistItems(Guid buyerId);
    public Task AddToWishlist(Guid buyerId, Guid clothId);
    public Task FromWishlistToCart(Guid buyerId, Guid wishlistId);
    public Task DeleteWishlistItem(Guid buyerId, Guid wishlistId);
}
