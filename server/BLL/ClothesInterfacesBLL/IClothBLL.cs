using ClothDTOs;
using Microsoft.AspNetCore.Http;

namespace ClothesInterfacesBLL;

public interface IClothBLL
{
    public Task<List<ClothDTO>> GetAll();
    public Task<ClothDTO> GetById(Guid id);
    public Task AddCloth(ClothAddDTO cloth, IEnumerable<IFormFile> file);
    public Task UpdateCloth(ClothAddDTO clothUpdt);
    public Task DeleteCloth(Guid id);
    public Task<List<ClothDTO>> GetMensClothing();
    public Task<List<ClothDTO>> GetWomensClothing();
}
