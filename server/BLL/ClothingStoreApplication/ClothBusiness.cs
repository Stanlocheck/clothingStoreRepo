using AutoMapper;
using ClothDomain;
using ClothDTOs;
using ClothesInterfacesBLL;
using ClothesInterfacesDAL;
using ClothingStorePersistence;

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
        try {
            var cloth = await _clothDAO.GetAll();
            var clothDTO = _clothDTO.Map<List<Cloth>, List<ClothDTO>>(cloth);
            return clothDTO;
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
     }

     public async Task<ClothDTO> GetById(Guid id){
        try{
            var cloth = await _clothDAO.GetById(id);
            var clothDTO = _clothDTO.Map<Cloth, ClothDTO>(cloth);
            return clothDTO;
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
     }

     public async Task AddCloth(ClothAddDTO cloth){
        try{
            var clothAddDTO = _clothAddDTO.Map<ClothAddDTO, Cloth>(cloth);
            await _clothDAO.AddCloth(clothAddDTO);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
     }

     public async Task UpdateCloth(ClothAddDTO cloth){
        try{
            var clothUpdateDTO = _clothAddDTO.Map<ClothAddDTO, Cloth>(cloth);
            await _clothDAO.AddCloth(clothUpdateDTO);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
     }

     public async Task DeleteCloth(Guid id){
        try{
            await _clothDAO.DeleteCloth(id);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
     }
}
