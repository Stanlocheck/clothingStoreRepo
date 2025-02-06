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
            cart = new Cart { BuyerId = buyerId };
            await _context.Carts.AddAsync(cart);
        }

        return cart.Items.ToList();
    }

    public async Task AddAmountOfCartItem(Guid buyerId, Guid cartItemId){
        var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(cart == null){
            cart = new Cart { BuyerId = buyerId };
            await _context.Carts.AddAsync(cart);
        }

        var existingItem = cart.Items.FirstOrDefault(ci => ci.Id == cartItemId);
        if(existingItem == null){
            throw new Exception("Продукт не найден.");
        }
        
        SqlDAO getById = new SqlDAO(_context);
        var cloth = await getById.GetById(existingItem.ClothId);

        existingItem.Amount++;
        existingItem.Price+=cloth.Price;
        cart.Price+=cloth.Price;

        await _context.SaveChangesAsync();
    }

    public async Task ReduceAmountOfCartItem(Guid buyerId, Guid cartItemId){
        var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(cart == null){
            cart = new Cart { BuyerId = buyerId };
            await _context.Carts.AddAsync(cart);
        }

        var existingItem = cart.Items.FirstOrDefault(ci => ci.Id == cartItemId);
        if(existingItem == null){
            throw new Exception("Продукт не найден.");
        }

        SqlDAO getById = new SqlDAO(_context);
        var cloth = await getById.GetById(existingItem.ClothId);

        existingItem.Amount--;
        existingItem.Price-=cloth.Price;
        cart.Price-=cloth.Price;

        if(existingItem.Amount == 0){
            _context.CartItems.Remove(existingItem);
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteCartItem(Guid buyerId, Guid cartItemId){
        var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(cart == null){
            cart = new Cart { BuyerId = buyerId };
            await _context.Carts.AddAsync(cart);
        }

        var existingItem = cart.Items.FirstOrDefault(ci => ci.Id == cartItemId);
        if(existingItem == null){
            throw new Exception("Продукт не найден.");
        }

        cart.Price-=existingItem.Price;
        _context.CartItems.Remove(existingItem);

        await _context.SaveChangesAsync();
    }

    public async Task SelectCartItem(Guid buyerId, Guid cartItemId){
        var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(cart == null){
            cart = new Cart { BuyerId = buyerId };
            await _context.Carts.AddAsync(cart);
        }

        var existingItem = cart.Items.FirstOrDefault(ci => ci.Id == cartItemId);
        if(existingItem == null){
            throw new Exception("Продукт не найден.");
        }

        if(existingItem.Selected == true){
            existingItem.Selected = false;
            cart.Price-=existingItem.Price;
        }
        else{
            existingItem.Selected = true;
            cart.Price+=existingItem.Price;
        }
        
        await _context.SaveChangesAsync();
    }
}
