using ClothDomain;
using ClothesInterfacesDAL;
using Microsoft.EntityFrameworkCore;

namespace ClothingStorePersistence;

public class SqlImageDAO : IImagesDAO
{
    private readonly ApplicationDbContext _context;

    public SqlImageDAO(ApplicationDbContext context){
        _context = context;
    }

    public async Task<ClothImage> GetImage(Guid imageId){
        var image = await _context.ClothImages.FirstOrDefaultAsync(_image => _image.Id == imageId);
        if(image == null){
            throw new Exception("Изображение не найдено.");
        }

        return image;
    }

    public async Task AddImage(ClothImage image, Guid clothId){
        var getById = new SqlDAO(_context);
        var cloth = await getById.GetById(clothId);

        cloth.Images.Add(image);
        
        await _context.SaveChangesAsync();
    }

    public async Task DeleteImage(Guid imageId){
        var image = await GetImage(imageId);

        _context.ClothImages.Remove(image);
        await _context.SaveChangesAsync();
    }
}
