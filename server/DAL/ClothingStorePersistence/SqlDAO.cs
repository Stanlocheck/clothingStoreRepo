using System.Text.Json;
using System.Text.Json.Serialization;
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
        return await _context.Clothes.Include(c => c.Images).ToListAsync();
    }

    public async Task<Cloth> GetById(Guid id){
        Cloth? cloth = null;
        var clothString = await _cache.GetStringAsync(id.ToString());
        if(clothString != null){
            var optionsCache = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            cloth = JsonSerializer.Deserialize<Cloth>(clothString, optionsCache);
        }
        
        if(cloth == null){
            cloth = await _context.Clothes.Include(c => c.Images).FirstOrDefaultAsync(_cloth => _cloth.Id == id);
            if(cloth == null){
                throw new Exception("Продукт не найден.");
            }
            var optionsCache = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                WriteIndented = true
            };
            clothString = JsonSerializer.Serialize(cloth, optionsCache);
            await _cache.SetStringAsync(cloth.Id.ToString(), clothString, new DistributedCacheEntryOptions{
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
            });
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

        var optionsCache = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };
        var clothCache = JsonSerializer.Serialize(cloth, optionsCache);
        await _cache.SetStringAsync(cloth.Id.ToString(), clothCache, new DistributedCacheEntryOptions{
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
        });
    }

    public async Task UpdateCloth(Cloth clothUpdt){
        _context.Clothes.Update(clothUpdt);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteCloth(Guid id){
        var cloth = await GetById(id);
        var optionsCache = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };
        var clothString = JsonSerializer.Serialize(cloth, optionsCache);

        _cache.Remove(clothString);
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

    public async Task DeleteImage(Guid clothId, IEnumerable<Guid> images){
        var cloth = await GetById(clothId);

        foreach(var imageId in images){
            var image = await GetImage(imageId);
            cloth.Images.Remove(image);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<ClothImage> GetImage(Guid imageId){
        var image = await _context.ClothImages.FirstOrDefaultAsync(_image => _image.Id == imageId);
        if(image == null){
            throw new Exception("Изображение не найдено.");
        }

        return image;
    }
}
