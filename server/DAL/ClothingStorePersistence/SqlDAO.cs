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

    public async Task AddCloth(Cloth cloth, IEnumerable<(byte[] imageData, string imageContentType)> images){
        await _context.Clothes.AddAsync(cloth);
        await _context.SaveChangesAsync();

        foreach(var (data, contentType) in images){
                cloth.Images.Add(new ClothImage{
                Data = data,
                ContentType = contentType,
                ClothId = cloth.Id
            });
        }

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

    public async Task AddImage(Guid clothId, IEnumerable<(byte[] imageData, string imageContentType)> images){
        var cloth = await GetById(clothId);

        foreach(var (data, contentType) in images){
                cloth.Images.Add(new ClothImage{
                Data = data,
                ContentType = contentType,
                ClothId = clothId
            });
        }

        await _context.SaveChangesAsync();
    }
}
