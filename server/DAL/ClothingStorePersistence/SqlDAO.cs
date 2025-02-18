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
        return await _context.Clothes.Include(c => c.Images).ToListAsync();
    }

    public async Task<Cloth> GetById(Guid id){
        var cloth = await _context.Clothes.Include(c => c.Images).FirstOrDefaultAsync(_cloth => _cloth.Id == id);
        if(cloth == null){
            throw new Exception("Продукт не найден.");
        }

        return cloth;
    }

    public async Task AddCloth(Cloth cloth, byte[] imageData, string imageContentType){
        await _context.Clothes.AddAsync(cloth);

        await _context.SaveChangesAsync();

        cloth.Images.Add(new ClothImage{
            Data = imageData,
            ContentType = imageContentType,
            ClothId = cloth.Id
        });

        await _context.SaveChangesAsync();
    }

    public async Task UpdateCloth(Cloth clothUpdt){
        _context.Clothes.Update(clothUpdt);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteCloth(Guid id){
        var cloth = await GetById(id);

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
