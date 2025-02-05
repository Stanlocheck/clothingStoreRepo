using System;
using ClothDomain;
using ClothesInterfacesDAL;
using Microsoft.EntityFrameworkCore;

namespace ClothingStorePersistence;

public class SqlDAO : IClothesDAO, ICartDAO, IWishlistDAO
{
    private readonly ApplicationDbContext _context;

    public SqlDAO(ApplicationDbContext context){
        _context = context;
    }
    
    public async Task<List<Cloth>> GetAll(){
        return await _context.Clothes.ToListAsync();
    }
    public async Task<Cloth> GetById(Guid id){
        var cloth = await _context.Clothes.FirstOrDefaultAsync(_cloth => _cloth.Id == id);
        if(cloth == null){
            throw new Exception("Продукт не найден.");
        }

        return cloth;
    }
    public async Task AddCloth(Cloth cloth){
        await _context.Clothes.AddAsync(cloth);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateCloth(Cloth clothUpdt, Guid id){
        var cloth = await _context.Clothes.FindAsync(id);  
        if(cloth == null){
            throw new Exception("Продукт не найден.");
        } 

        cloth.Price = clothUpdt.Price;
        cloth.Size = clothUpdt.Size;
        cloth.CountryOfOrigin = clothUpdt.CountryOfOrigin;
        cloth.Brand = clothUpdt.Brand;
        cloth.Material = clothUpdt.Material;
        cloth.Season = clothUpdt.Season;
        cloth.Type = clothUpdt.Type;   
        cloth.Sex = clothUpdt.Sex;  

        await _context.SaveChangesAsync();
    }
    public async Task DeleteCloth(Guid id){
        var cloth = await _context.Clothes.FindAsync(id);
        if(cloth == null){
            throw new Exception("Продукт не найден.");
        }

        _context.Clothes.Remove(cloth);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<Cloth>> GetMensClothing(){
        return await _context.Clothes.Where(m => m.Sex == (Gender)0).ToListAsync();
    }

    public async Task<List<Cloth>> GetWomensClothing(){
        return await _context.Clothes.Where(w => w.Sex == (Gender)1).ToListAsync();
    }

    public async Task AddToCart(Guid buyerId, Guid clothId){
        var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(cart == null){
            cart = new Cart { BuyerId = buyerId };
            await _context.Carts.AddAsync(cart);
        }

        var existingItem = cart.Items.FirstOrDefault(ci => ci.ClothId == clothId);
        if(existingItem != null){
            existingItem.Amount++;
        }
        else {
            cart.Items.Add(new CartItem {ClothId = clothId, Amount = 1});
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<CartItem>> GetCartItems(Guid buyerId){
        var cart = await _context.Carts.Include(c => c.Items).ThenInclude(ci => ci.Cloth).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(cart == null){
            throw new Exception("Корзина не найдена.");
        }

        return cart.Items.ToList();
    }

    public async Task AddAmountOfCartItem(Guid buyerId, Guid cartId){
        var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(cart == null){
            throw new Exception("Корзина не найдена.");
        }

        var existingItem = cart.Items.FirstOrDefault(ci => ci.CartId == cartId);
        if(existingItem == null){
            throw new Exception("Продукт не найден.");
        }
        else{
            existingItem.Amount++;
        }

        await _context.SaveChangesAsync();
    }

    public async Task ReduceAmountOfCartItem(Guid buyerId, Guid cartId){
        var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(cart == null){
            throw new Exception("Корзина не найдена.");
        }

        var existingItem = cart.Items.FirstOrDefault(ci => ci.CartId == cartId);
        if(existingItem == null){
            throw new Exception("Продукт не найден.");
        }
        else{
            existingItem.Amount--;
        }

        if(existingItem.Amount == 0){
            _context.CartItems.Remove(existingItem);
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteCartItem(Guid buyerId, Guid cartId){
        var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(cart == null){
            throw new Exception("Корзина не найдена");
        }

        var existingItem = cart.Items.FirstOrDefault(ci => ci.CartId == cartId);
        if(existingItem == null){
            throw new Exception("Продукт не найден.");
        }
        else{
            _context.CartItems.Remove(existingItem);
        }

        await _context.SaveChangesAsync();
    }

    public async Task AddToWishlist(Guid buyerId, Guid clothId){
        var wishlist = await _context.Wishlists.Include(c => c.Items).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(wishlist == null){
            wishlist = new Wishlist { BuyerId = buyerId };
            await _context.Wishlists.AddAsync(wishlist);
        }

        var existingItem = wishlist.Items.FirstOrDefault(ci => ci.ClothId == clothId);
        if(existingItem != null){
            throw new Exception("Продукт уже в вишлисте.");
        }
        
        wishlist.Items.Add(new WishlistItem { ClothId = clothId });
        await _context.SaveChangesAsync();
    }

    public async Task<List<WishlistItem>> GetWishlistItems(Guid buyerId){
        var wishlist = await _context.Wishlists.Include(c => c.Items).ThenInclude(ci => ci.Cloth).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(wishlist == null){
            throw new Exception("Вишлист не найден.");
        }

        return wishlist.Items.ToList();
    }

    public async Task FromWishlistToCart(Guid buyerId, Guid wishlistId){
        var wishlist = await _context.Wishlists.Include(c => c.Items).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(wishlist == null){
            throw new Exception("Вишлист не найден.");
        }

        var existingWishlistItem = wishlist.Items.FirstOrDefault(ci => ci.WishlistId == wishlistId);
        if(existingWishlistItem == null){
            throw new Exception("Продукт не найден.");
        }
        
        await AddToCart(buyerId, existingWishlistItem.ClothId);
        _context.WishlistItems.Remove(existingWishlistItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteWishlistItem(Guid buyerId, Guid wishlistId){
        var wishlist = await _context.Wishlists.Include(c => c.Items).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(wishlist == null){
            throw new Exception("Вишлист не найден");
        }

        var existingItem = wishlist.Items.FirstOrDefault(ci => ci.WishlistId == wishlistId);
        if(existingItem == null){
            throw new Exception("Продукт не найден.");
        }

        _context.WishlistItems.Remove(existingItem);
        await _context.SaveChangesAsync();
    } 
}
