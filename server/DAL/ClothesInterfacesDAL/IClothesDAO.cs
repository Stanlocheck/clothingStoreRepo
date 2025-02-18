using ClothDomain;

namespace ClothesInterfacesDAL;

public interface IClothesDAO
{
    public Task<List<Cloth>> GetAll();
    public Task<Cloth> GetById(Guid id);
    public Task AddCloth(Cloth cloth, byte[] imageData, string imageContentType);
    public Task UpdateCloth(Cloth clothUpdt);
    public Task DeleteCloth(Guid id);
    public Task<List<Cloth>> GetMensClothing();
    public Task<List<Cloth>> GetWomensClothing();
}
