using System;
using ClothDomain;
using ClothesInterfacesDAL;
using Microsoft.EntityFrameworkCore;

namespace ClothingStorePersistence;

public class SqlDAO : IClothesDAO, ICartDAO
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
            throw new Exception("Object not found.");
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
            throw new Exception("Object not found.");
        } 

        cloth.Price = clothUpdt.Price;
        cloth.Size = clothUpdt.Size;
        cloth.Manufacturer = clothUpdt.Manufacturer;
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
            throw new Exception("Object not found.");
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

    /*public async Task<List<Cloth>> FilterBySex(Gender gender){
        if(gender != (Gender)0 || gender != (Gender)1){
            throw new Exception("Неверно указан пол.");
        }

        return await _context.Clothes.Where(f => f.Sex == gender).ToListAsync();
    }*/

    public async Task AddCartItem(Guid buyerId, Guid clothId){
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
            throw new Exception("Object not found.");
        }

        return cart.Items.ToList();
    }

    public async Task AddAmount(Guid buyerId, Guid clothId){
        var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(cart == null){
            throw new Exception("Object not found.");
        }

        var existingItem = cart.Items.FirstOrDefault(ci => ci.ClothId == clothId);
        if(existingItem == null){
            throw new Exception("Object not found.");
        }
        else{
            existingItem.Amount++;
        }

        await _context.SaveChangesAsync();
    }

    public async Task ReduceAmount(Guid buyerId, Guid clothId){
        var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(cart == null){
            throw new Exception("Object not found.");
        }

        var existingItem = cart.Items.FirstOrDefault(ci => ci.ClothId == clothId);
        if(existingItem == null){
            throw new Exception("Object not found.");
        }
        else{
            existingItem.Amount--;
        }

        if(existingItem.Amount == 0){
            _context.Remove(existingItem);
        }

        await _context.SaveChangesAsync();
    }
}
