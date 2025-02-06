using ClothDomain;
using ClothesInterfacesDAL;
using Microsoft.EntityFrameworkCore;

namespace ClothingStorePersistence;

public class SqlBuyersDAO : IBuyersDAO
{
    private readonly ApplicationDbContext _context;

    public SqlBuyersDAO (ApplicationDbContext context){
        _context = context;
    }

    public async Task<List<Buyer>> GetAll(){
        return await _context.Buyers.ToListAsync();
    }
    public async Task<Buyer> GetById(Guid id){
        var buyer = await _context.Buyers.FirstOrDefaultAsync(_buyer => _buyer.Id == id);
        if(buyer == null){
            throw new Exception("Пользователь не найден.");
        }

        return buyer;
    }
    public async Task AddBuyer(Buyer buyer){
        await _context.Buyers.AddAsync(buyer);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateBuyer(Buyer buyerUpdt, Guid id){
        var buyer = await _context.Buyers.FindAsync(id);  
        if(buyer == null){
            throw new Exception("Пользователь не найден.");
        } 

        buyer.FirstName = buyerUpdt.FirstName;
        buyer.LastName = buyerUpdt.LastName;
        buyer.DateOfBirth = buyerUpdt.DateOfBirth;
        buyer.Sex = buyerUpdt.Sex;
        buyer.PhoneNumber = buyerUpdt.PhoneNumber;
        buyer.City = buyerUpdt.City;
        buyer.StreetAddress = buyerUpdt.StreetAddress;
        buyer.ApartmentNumber = buyerUpdt.ApartmentNumber;

        await _context.SaveChangesAsync();
    }
    public async Task DeleteBuyer(Guid id){
        var buyer = await _context.Buyers.FindAsync(id);
        if(buyer == null){
            throw new Exception("Пользователь не найден.");
        }

        _context.Buyers.Remove(buyer);
        await _context.SaveChangesAsync();
    }
}
