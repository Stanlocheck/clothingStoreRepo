using ClothDomain;
using ClothesInterfacesDAL;
using Microsoft.EntityFrameworkCore;

namespace ClothingStorePersistence;

public class SqlCartDAO : ICartDAO
{
    private readonly ApplicationDbContext _context;

    public SqlCartDAO(ApplicationDbContext context){
        _context = context;
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
}
