using ClothDTOs;
using ClothDTOs.ClothesDTOs;
using Microsoft.AspNetCore.Http;

namespace ClothesInterfacesBLL;

public interface IClothBLL
{
    public Task<List<ClothDTO>> GetAll();
    public Task<ClothDTO> GetById(Guid id);
    public Task AddCloth(ClothAddDTO cloth, IEnumerable<IFormFile> files);
    public Task UpdateCloth(ClothAddDTO clothUpdt);
    public Task DeleteCloth(Guid id);
    public Task<List<ClothDTO>> GetMensClothing();
    public Task<List<ClothDTO>> GetWomensClothing();
    public Task AddImage(Guid clothId, IEnumerable<IFormFile> files);
    public Task DeleteImage(Guid clothId, IEnumerable<Guid> files);
}
