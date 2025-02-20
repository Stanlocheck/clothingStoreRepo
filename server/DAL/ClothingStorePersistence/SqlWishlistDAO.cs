using ClothesInterfacesDAL;
using ClothDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace ClothingStorePersistence;

public class SqlWishlistDAO : IWishlistDAO
{
    private readonly ApplicationDbContext _context;
    private readonly IDistributedCache _cache;

    public SqlWishlistDAO(ApplicationDbContext context, IDistributedCache cache){
        _context = context;
        _cache = cache;
    }

    public async Task<List<Wishlist>> GetAllWishlists(){
        return await _context.Wishlists.Include(w => w.Items).ToListAsync();
    }

    public async Task<Wishlist> GetWishlist(Guid buyerId){
        var wishlist = await _context.Wishlists.Include(w => w.Items).FirstOrDefaultAsync(w => w.BuyerId == buyerId);
        if(wishlist == null){
            await CreateWishlist(buyerId);
            return new Wishlist { BuyerId = buyerId };
        }

        return wishlist;
    }

    public async Task CreateWishlist(Guid buyerId){
        var wishlist = new Wishlist { BuyerId = buyerId };

        await _context.Wishlists.AddAsync(wishlist);
        await _context.SaveChangesAsync();
    }

    public async Task<List<WishlistItem>> GetAllWishlistItems(Guid buyerId){
        var wishlist = await _context.Wishlists.Include(w => w.Items).ThenInclude(wi => wi.Cloth).FirstOrDefaultAsync(w => w.BuyerId == buyerId);
        if(wishlist == null){
            await CreateWishlist(buyerId);
            return new List<WishlistItem>();
        }

        return wishlist.Items.ToList();
    }

    public async Task<WishlistItem> GetWishlistItem(Guid buyerId, Guid wishlistItemId){
        var wishlist = await GetWishlist(buyerId);
        var wishlistItem = wishlist.Items.FirstOrDefault(wi => wi.Id == wishlistItemId);
        if(wishlistItem == null){
            throw new Exception("Продукт не найден.");
        }

        return wishlistItem;
    }

    public async Task AddToWishlist(Guid buyerId, Guid clothId){
        var wishlist = await GetWishlist(buyerId);

        var getById = new SqlDAO(_context, _cache);
        var cloth = await getById.GetById(clothId);

        var wishlistItem = wishlist.Items.FirstOrDefault(wi => wi.ClothId == clothId);
        if(wishlistItem != null){
            throw new Exception("Продукт уже в вишлисте.");
        }
        
        wishlist.Items.Add(new WishlistItem { ClothId = clothId, Price = cloth.Price });
        await _context.SaveChangesAsync();
    }

    public async Task FromWishlistToCart(Guid buyerId, Guid wishlistItemId){
        var wishlistItem = await GetWishlistItem(buyerId, wishlistItemId);

        var addToCart = new SqlCartDAO(_context, _cache);
        await addToCart.AddToCart(buyerId, wishlistItem.ClothId);

        _context.WishlistItems.Remove(wishlistItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteWishlistItem(Guid buyerId, Guid wishlistItemId){
        var wishlistItem = await GetWishlistItem(buyerId, wishlistItemId);

        _context.WishlistItems.Remove(wishlistItem);
        await _context.SaveChangesAsync();
    }
}
