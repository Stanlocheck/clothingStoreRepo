using ClothDomain;
using ClothesInterfacesDAL;
using Microsoft.EntityFrameworkCore;

namespace ClothingStorePersistence;

public class SqlDAO : IClothesDAO
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
        
        var cloth = await GetById(clothId);

        var existingItem = cart.Items.FirstOrDefault(ci => ci.ClothId == clothId);
        if(existingItem != null){
            existingItem.Amount++;
            existingItem.Price+=cloth.Price;
            cart.Price+=cloth.Price;
        }
        else {
            cart.Items.Add(new CartItem { ClothId = clothId, Amount = 1, Selected = true, Price = cloth.Price });
            cart.Price+=cloth.Price;
        }

        await _context.SaveChangesAsync();
    }

    public async Task AddToWishlist(Guid buyerId, Guid clothId){
        var cloth = await GetById(clothId);

        var wishlist = await _context.Wishlists.Include(c => c.Items).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(wishlist == null){
            wishlist = new Wishlist { BuyerId = buyerId };
            await _context.Wishlists.AddAsync(wishlist);
        }

        var existingItem = wishlist.Items.FirstOrDefault(ci => ci.ClothId == clothId);
        if(existingItem != null){
            throw new Exception("Продукт уже в вишлисте.");
        }
        
        wishlist.Items.Add(new WishlistItem { ClothId = clothId, Price = cloth.Price });
        await _context.SaveChangesAsync();
    }
}
