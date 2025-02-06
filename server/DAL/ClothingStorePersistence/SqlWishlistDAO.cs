using ClothesInterfacesDAL;
using ClothDomain;
using Microsoft.EntityFrameworkCore;

namespace ClothingStorePersistence;

public class SqlWishlistDAO : IWishlistDAO
{
    private readonly ApplicationDbContext _context;

    public SqlWishlistDAO(ApplicationDbContext context){
        _context = context;
    }

    public async Task<List<WishlistItem>> GetWishlistItems(Guid buyerId){
        var wishlist = await _context.Wishlists.Include(c => c.Items).ThenInclude(ci => ci.Cloth).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(wishlist == null){
            wishlist = new Wishlist { BuyerId = buyerId };
            await _context.Wishlists.AddAsync(wishlist);
        }

        return wishlist.Items.ToList();
    }

    public async Task FromWishlistToCart(Guid buyerId, Guid wishlistItemId){
        var wishlist = await _context.Wishlists.Include(c => c.Items).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(wishlist == null){
            wishlist = new Wishlist { BuyerId = buyerId };
            await _context.Wishlists.AddAsync(wishlist);
        }

        var existingWishlistItem = wishlist.Items.FirstOrDefault(ci => ci.Id == wishlistItemId);
        if(existingWishlistItem == null){
            throw new Exception("Продукт не найден.");
        }
        
        SqlDAO addToCart = new SqlDAO(_context);
        await addToCart.AddToCart(buyerId, existingWishlistItem.ClothId);
        _context.WishlistItems.Remove(existingWishlistItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteWishlistItem(Guid buyerId, Guid wishlistItemId){
        var wishlist = await _context.Wishlists.Include(c => c.Items).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(wishlist == null){
            wishlist = new Wishlist { BuyerId = buyerId };
            await _context.Wishlists.AddAsync(wishlist);
        }

        var existingItem = wishlist.Items.FirstOrDefault(ci => ci.Id == wishlistItemId);
        if(existingItem == null){
            throw new Exception("Продукт не найден.");
        }

        _context.WishlistItems.Remove(existingItem);
        await _context.SaveChangesAsync();
    }
}
