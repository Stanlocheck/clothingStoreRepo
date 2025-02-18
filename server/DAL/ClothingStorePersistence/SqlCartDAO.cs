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

    public async Task<List<Cart>> GetAllCarts(){
        return await _context.Carts.Include(c => c.Items).ToListAsync();
    }

    public async Task<Cart> GetCart(Guid buyerId){
        var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(cart == null){
            await CreateCart(buyerId);
            return new Cart { BuyerId = buyerId, Price = 0, Amount = 0 };
        }

        return cart;
    }

    public async Task CreateCart(Guid buyerId){
        var cart = new Cart { BuyerId = buyerId, Price = 0, Amount = 0 };

        await _context.Carts.AddAsync(cart);
        await _context.SaveChangesAsync();
    }

    public async Task<CartItem> GetCartItem(Guid buyerId, Guid cartItemId){
        var cart = await GetCart(buyerId);

        var cartItem = cart.Items.FirstOrDefault(ci => ci.Id == cartItemId);
        if(cartItem == null){
            throw new Exception("Продукт не найден.");
        }

        return cartItem;
    }

    public async Task AddToCart(Guid buyerId, Guid clothId){
        var cart = await GetCart(buyerId);

        var getById = new SqlDAO(_context);
        var cloth = await getById.GetById(clothId);

        var cartItem = cart.Items.FirstOrDefault(ci => ci.ClothId == clothId);
        if(cartItem != null){
            cartItem.Amount++;
            cartItem.Price+=cloth.Price;
            cart.Price+=cloth.Price;
            cart.Amount++;
        }
        else {
            cart.Items.Add(new CartItem { 
                ClothId = clothId, 
                Amount = 1, 
                Selected = true, 
                Price = cloth.Price 
            });
            cart.Price+=cloth.Price;
            cart.Amount++;
        }

        await _context.SaveChangesAsync();
    }

    public async Task AddAmountOfCartItem(Guid buyerId, Guid cartItemId){
        var cartItem = await GetCartItem(buyerId, cartItemId);

        var getById = new SqlDAO(_context);
        var cloth = await getById.GetById(cartItem.ClothId);

        cartItem.Amount++;
        cartItem.Price+=cloth.Price;
        if(cartItem.Selected == true){
            cartItem.Cart.Price+=cloth.Price;
            cartItem.Cart.Amount++;
        }

        await _context.SaveChangesAsync();
    }

    public async Task ReduceAmountOfCartItem(Guid buyerId, Guid cartItemId){
        var cartItem = await GetCartItem(buyerId, cartItemId);

        var getById = new SqlDAO(_context);
        var cloth = await getById.GetById(cartItem.ClothId);

        cartItem.Amount--;
        cartItem.Price-=cloth.Price;
        if(cartItem.Selected == true){
            cartItem.Cart.Price-=cloth.Price;
            cartItem.Cart.Amount--;
        }

        if(cartItem.Amount == 0){
            _context.CartItems.Remove(cartItem);
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteCartItem(Guid buyerId, Guid cartItemId){
        var cartItem = await GetCartItem(buyerId, cartItemId);

        if(cartItem.Selected == true){
            cartItem.Cart.Price-=cartItem.Price;
            cartItem.Cart.Amount-=cartItem.Amount;
        }
        
        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();
    }

    public async Task SelectCartItem(Guid buyerId, Guid cartItemId){
        var cartItem = await GetCartItem(buyerId, cartItemId);

        if(cartItem.Selected == true){
            cartItem.Selected = false;
            cartItem.Cart.Price-=cartItem.Price;
            cartItem.Cart.Amount-=cartItem.Amount;
        }
        else{
            cartItem.Selected = true;
            cartItem.Cart.Price+=cartItem.Price;
            cartItem.Cart.Amount+=cartItem.Amount;
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<CartItem>> GetAllSelectedItems(Guid buyerId){
        var cart = await GetCart(buyerId);
        return cart.Items.Where(ci => ci.Selected == true).ToList();
    }
}
