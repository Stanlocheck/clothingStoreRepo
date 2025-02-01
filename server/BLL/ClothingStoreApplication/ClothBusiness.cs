using System.Runtime.InteropServices;
using AutoMapper;
using ClothDomain;
using ClothDTOs;
using ClothesInterfacesBLL;
using ClothesInterfacesDAL;
using ClothingStorePersistence;

namespace ClothingStoreApplication;

public class ClothBusiness : IClothBLL, ICartBLL
{
    private Mapper _clothDTO;
    private Mapper _clothAddDTO;
    private Mapper _cartItemDTO;
    private readonly IClothesDAO _clothDAO;
    private readonly ICartDAO _cartDAO;
    public ClothBusiness(IClothesDAO clothDAO, ICartDAO cartDAO){
        _clothDAO = clothDAO;
        _cartDAO = cartDAO;

        var _clothDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Cloth, ClothDTO>().ReverseMap());
        _clothDTO = new Mapper(_clothDtoMapping);

        var _clothAddDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Cloth, ClothAddDTO>().ReverseMap());
        _clothAddDTO = new Mapper(_clothAddDtoMapping);

        var _cartItemDtoMapping = new MapperConfiguration(cfg => {
            cfg.CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.Cloth, opt => opt.MapFrom(src => src.Cloth))
                .ForMember(dest => dest.Cart, opt => opt.MapFrom(src => src.Cart));
            cfg.CreateMap<Cart, CartDTO>()
                .ForMember(dest => dest.Buyer, opt => opt.MapFrom(src => src.Buyer))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            cfg.CreateMap<Buyer, BuyerDTO>();
            cfg.CreateMap<Cloth, ClothDTO>();
        });
        _cartItemDTO = new Mapper(_cartItemDtoMapping);
    }

    public async Task<List<ClothDTO>> GetAll(){
        try {
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
                Manufacturer = clothInfo.Manufacturer,
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

     public async Task UpdateCloth(ClothAddDTO clothInfo, Guid id){
        try{
            var cloth = new ClothAddDTO {
                Price = clothInfo.Price,
                Type = clothInfo.Type,
                Brand = clothInfo.Brand,
                Season = clothInfo.Season,
                Size = clothInfo.Size,
                Material = clothInfo.Material,
                Manufacturer = clothInfo.Manufacturer,
                Sex = clothInfo.Sex.ToUpper()
            };
            Enum.Parse<Gender>(cloth.Sex);
            var clothUpdateDTO = _clothAddDTO.Map<ClothAddDTO, Cloth>(cloth);
            await _clothDAO.UpdateCloth(clothUpdateDTO, id);
        }
        catch(ArgumentException){
            throw new Exception("Неверно указан пол.");
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

     /*public async Task<List<ClothDTO>> FilterBySex(Gender gender){
        try{
            var cloth = await _clothDAO.FilterBySex(gender);
            return _clothDTO.Map<List<Cloth>, List<ClothDTO>>(cloth);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
     }*/

     public async Task AddCartItem(Guid buyerId, Guid clothId){
        try{
            await _cartDAO.AddCartItem(buyerId, clothId);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
     }

     public async Task<List<CartItemDTO>> GetCartItems(Guid buyerId){
        try{
            var cart = await _cartDAO.GetCartItems(buyerId);
            return _cartItemDTO.Map<List<CartItemDTO>>(cart);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
     }
}
