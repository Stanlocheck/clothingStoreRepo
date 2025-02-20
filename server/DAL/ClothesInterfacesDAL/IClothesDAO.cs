using ClothDomain;

namespace ClothesInterfacesDAL;

public interface IClothesDAO
{
    public Task<List<Cloth>> GetAll();
    public Task<Cloth> GetById(Guid id);
    public Task AddCloth(Cloth cloth, IEnumerable<(byte[] imageData, string imageContentType)> images);
    public Task UpdateCloth(Cloth clothUpdt);
    public Task DeleteCloth(Guid id);
    public Task<List<Cloth>> GetMensClothing();
    public Task<List<Cloth>> GetWomensClothing();
    public Task AddImage(Guid clothId, IEnumerable<(byte[] imageData, string imageContentType)> images);
    public Task DeleteImage(Guid clothId, IEnumerable<Guid> images);
    public Task<ClothImage> GetImage(Guid imageId);
}
