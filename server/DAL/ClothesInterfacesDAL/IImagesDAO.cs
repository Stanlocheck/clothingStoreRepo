using ClothDomain;

namespace ClothesInterfacesDAL;

public interface IImagesDAO
{
    public Task<ClothImage> GetImage(Guid imageId);
    public Task AddImage(ClothImage image, Guid clothId);
    public Task DeleteImage(Guid imageId);
}
