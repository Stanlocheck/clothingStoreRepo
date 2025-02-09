using ClothDomain;
using ClothesInterfacesDAL;
using Microsoft.EntityFrameworkCore;

namespace ClothingStorePersistence;

public class SqlOrderDAO : IOrderDAO
{
    private readonly ApplicationDbContext _context;

    public SqlOrderDAO(ApplicationDbContext context){
        _context = context;
    }

    public async Task<List<Order>> GetAllOrders(Guid buyerId){
        return await _context.Orders.Include(o => o.Items).ToListAsync();
    }

    public async Task<Order> GetOrder(Guid buyerId, Guid orderId){
        var order = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == orderId);
        if(order == null){
            throw new Exception("Заказ не найден.");
        }

        return order;
    }

    public async Task CreateOrder(Guid buyerId){
        var cart = await _context.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.BuyerId == buyerId);
        if(cart == null || cart.Items.ToList().Count == 0){
            throw new Exception("Корзина пустая.");
        }

        var sqlCartDAO = new SqlCartDAO(_context);
        var selectedItems = await sqlCartDAO.GetAllSelectedItems(buyerId);

        var order = new Order { 
            BuyerId = buyerId, 
            Price = cart.Price, 
            Amount = cart.Amount, 
            Status = OrderStatus.СОБИРАЕТСЯ, 
            OrderDate = DateTime.UtcNow 
        };
        foreach(var item in selectedItems){
            order.Items.Add(new OrderItem { 
                ClothId = item.ClothId, 
                Amount = item.Amount, 
                Price = item.Price 
            });
            await sqlCartDAO.DeleteCartItem(buyerId, item.Id);
        }

        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task SelectOrderStatus(Guid orderId, OrderStatus status){
        var order = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == orderId);
        if(order == null){
            throw new Exception("Заказ не найден.");
        }

        order.Status = status;
        await _context.SaveChangesAsync();
    }
}
