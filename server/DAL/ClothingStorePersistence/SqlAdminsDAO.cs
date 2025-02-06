using ClothesInterfacesDAL;
using ClothDomain;
using Microsoft.EntityFrameworkCore;

namespace ClothingStorePersistence;

public class SqlAdminsDAO : IAdminsDAO
{
    private readonly ApplicationDbContext _dbContext;

    public SqlAdminsDAO(ApplicationDbContext dbContext){
        _dbContext = dbContext;
    }

    public async Task<List<Admin>> GetAll(){
        return await _dbContext.Admins.ToListAsync();
    }

    public async Task<Admin> GetById(Guid id){
        var admin = await _dbContext.Admins.FirstOrDefaultAsync(_admin => _admin.Id == id);
        if(admin == null){
            throw new Exception("Пользователь не найден.");
        }

        return admin;
    }

    public async Task AddAdmin(Admin admin){
        await _dbContext.Admins.AddAsync(admin);
        await _dbContext.SaveChangesAsync();
    }
    public async Task UpdateAdmin(Admin adminUpdt, Guid id){
        var admin = await _dbContext.Admins.FindAsync(id);  
        if(admin == null){
            throw new Exception("Пользователь не найден.");
        } 

        admin.FirstName = adminUpdt.FirstName;
        admin.LastName = adminUpdt.LastName;
        admin.DateOfBirth = adminUpdt.DateOfBirth;
        admin.PhoneNumber = adminUpdt.PhoneNumber;

        await _dbContext.SaveChangesAsync();
    }
    public async Task DeleteAdmin(Guid id){
        var admin = await _dbContext.Admins.FindAsync(id);
        if(admin == null){
            throw new Exception("Пользователь не найден.");
        }

        _dbContext.Admins.Remove(admin);
        await _dbContext.SaveChangesAsync();
    }
}
