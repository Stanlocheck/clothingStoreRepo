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
}
