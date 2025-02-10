using AutoMapper;
using ClothDomain;
using ClothDTOs;
using ClothesInterfacesBLL;
using ClothesInterfacesDAL;

namespace ClothingStoreApplication;

public class ClothBusiness : IClothBLL
{
    private Mapper _clothDTO;
    private Mapper _clothAddDTO;
    private readonly IClothesDAO _clothDAO;
    public ClothBusiness(IClothesDAO clothDAO){
        _clothDAO = clothDAO;

        var _clothDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Cloth, ClothDTO>().ReverseMap());
        _clothDTO = new Mapper(_clothDtoMapping);

        var _clothAddDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Cloth, ClothAddDTO>().ReverseMap());
        _clothAddDTO = new Mapper(_clothAddDtoMapping);
    }

    public async Task<List<ClothDTO>> GetAll(){
        try{
            var cloth = await _clothDAO.GetAll();
            return _clothDTO.Map<List<Cloth>, List<ClothDTO>>(cloth);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task<ClothDTO> GetById(Guid id){
        try{
            var cloth = await _clothDAO.GetById(id);
            return _clothDTO.Map<Cloth, ClothDTO>(cloth);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task AddCloth(ClothAddDTO clothInfo){
        try{
            var cloth = new ClothAddDTO {
                Price = clothInfo.Price,
                Type = clothInfo.Type,
                Brand = clothInfo.Brand,
                Season = clothInfo.Season,
                Size = clothInfo.Size,
                Material = clothInfo.Material,
                CountryOfOrigin = clothInfo.CountryOfOrigin,
                Sex = clothInfo.Sex.ToUpper()
            };
            Enum.Parse<Gender>(cloth.Sex);
            var clothAddDTO = _clothAddDTO.Map<ClothAddDTO, Cloth>(cloth);                     
            await _clothDAO.AddCloth(clothAddDTO);
        }
        catch(ArgumentException){
            throw new Exception("Неверно указан пол.");
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    /*public async Task UpdateCloth(ClothAddDTO clothInfo, Guid id){
        try{
            Enum.Parse<Gender>(clothInfo.Sex);
            var clothUpdateDTO = _clothAddDTO.Map<ClothAddDTO, Cloth>(clothInfo);
            await _clothDAO.UpdateCloth(clothUpdateDTO, id);
        }
        catch(ArgumentException){
            throw new Exception("Неверно указан пол.");
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }*/

    public async Task DeleteCloth(Guid id){
        try{
            await _clothDAO.DeleteCloth(id);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<ClothDTO>> GetMensClothing(){
        try{
            var cloth = await _clothDAO.GetMensClothing();
            return _clothDTO.Map<List<Cloth>, List<ClothDTO>>(cloth);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<ClothDTO>> GetWomensClothing(){
        try{
            var cloth = await _clothDAO.GetWomensClothing();
            return _clothDTO.Map<List<Cloth>, List<ClothDTO>>(cloth);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }
}
