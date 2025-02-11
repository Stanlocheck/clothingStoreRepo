using System.Text.Json;
using ClothDomain;
using ClothesInterfacesDAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace ClothingStorePersistence;

public class SqlDAO : IClothesDAO
{
    private readonly ApplicationDbContext _context;
    private readonly IDistributedCache _cache;

    public SqlDAO(ApplicationDbContext context, IDistributedCache cache){
        _context = context;
        _cache = cache;
    }
    
    public async Task<List<Cloth>> GetAll(){
        return await _context.Clothes.ToListAsync();
    }

    public async Task<Cloth> GetById(Guid id){
        Cloth? cloth = null;
        var clothCache = await _cache.GetStringAsync(id.ToString());
        if(clothCache != null){
            cloth = JsonSerializer.Deserialize<Cloth>(clothCache);
        }

        if(cloth == null){
            cloth = await _context.Clothes.FirstOrDefaultAsync(_cloth => _cloth.Id == id);
            if(cloth == null){
                throw new Exception("Продукт не найден.");
            }

            clothCache = JsonSerializer.Serialize(cloth);
            await _cache.SetStringAsync(cloth.Id.ToString(), clothCache, new DistributedCacheEntryOptions{
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
            });
        }

        return cloth;
    }

    public async Task AddCloth(Cloth cloth){
        await _context.Clothes.AddAsync(cloth);

        var clothCache = JsonSerializer.Serialize(cloth);
        await _cache.SetStringAsync(cloth.Id.ToString(), clothCache, new DistributedCacheEntryOptions{
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
        });

        await _context.SaveChangesAsync();
    }

    /*public async Task UpdateCloth(Cloth clothUpdt, Guid id){
        var cloth = await GetById(id);

        cloth.Price = clothUpdt.Price;
        cloth.Size = clothUpdt.Size;
        cloth.CountryOfOrigin = clothUpdt.CountryOfOrigin;
        cloth.Brand = clothUpdt.Brand;
        cloth.Material = clothUpdt.Material;
        cloth.Season = clothUpdt.Season;
        cloth.Type = clothUpdt.Type;
        cloth.Sex = clothUpdt.Sex;

        var clothCache = JsonSerializer.Serialize(cloth);
        await _cache.SetStringAsync(cloth.Id.ToString(), clothCache, new DistributedCacheEntryOptions{
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
        });

        await _context.SaveChangesAsync();
    }*/
    
    public async Task DeleteCloth(Guid id){
        var cloth = await GetById(id);

        await _cache.RemoveAsync(id.ToString());
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
