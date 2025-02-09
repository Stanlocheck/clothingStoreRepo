using System.Security.Claims;
using AutoMapper;
using ClothDomain;
using ClothDTOs;
using ClothesInterfacesBLL;
using ClothesInterfacesDAL;
using Microsoft.AspNetCore.Http;

namespace ClothingStoreApplication;

public class BuyerBusiness : IBuyersBLL
{
    private readonly IBuyersDAO _buyersDAO;
    private Mapper _buyerDTO;
    private Mapper _buyerUpdateDTO;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BuyerBusiness(IBuyersDAO buyersDAO, IHttpContextAccessor httpContextAccessor){
        _buyersDAO = buyersDAO;
        _httpContextAccessor = httpContextAccessor;

        var _buyerDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Buyer, BuyerDTO>().ReverseMap());
        _buyerDTO = new Mapper(_buyerDtoMapping);

        var _buyerUpdateDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Buyer, BuyerUpdateDTO>().ReverseMap());
        _buyerUpdateDTO = new Mapper(_buyerUpdateDtoMapping);
    }

    private Guid GetLoggedInBuyerId(){
        var httpContext = _httpContextAccessor.HttpContext;
        if(httpContext == null){
            throw new Exception("HttpContext недоступен.");
        }

        var buyerIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if(buyerIdClaim == null || !Guid.TryParse(buyerIdClaim.Value, out var buyerId)){
            throw new Exception("Пользователь не авторизован.");
        }

        return buyerId;
    }

    public async Task<List<BuyerDTO>> GetAll(){
        try {
            var buyer = await _buyersDAO.GetAll();
            return _buyerDTO.Map<List<Buyer>, List<BuyerDTO>>(buyer);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

     public async Task<BuyerDTO> GetById(Guid id){
        try{
            var buyer = await _buyersDAO.GetById(id);
            return _buyerDTO.Map<Buyer, BuyerDTO>(buyer);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task UpdateBuyer(BuyerUpdateDTO buyerInfo){
        try{
            var buyerId = GetLoggedInBuyerId();

            var buyer = new BuyerUpdateDTO {
                FirstName = buyerInfo.FirstName,
                LastName = buyerInfo.LastName,
                DateOfBirth = buyerInfo.DateOfBirth,
                Sex = buyerInfo.Sex.ToUpper(),
                PhoneNumber = buyerInfo.PhoneNumber,
                City = buyerInfo.City,
                StreetAddress = buyerInfo.StreetAddress,
                ApartmentNumber = buyerInfo.ApartmentNumber,
                Balance = buyerInfo.Balance
            };
            Enum.Parse<Gender>(buyer.Sex);
            var buyerUpdateDTO = _buyerUpdateDTO.Map<BuyerUpdateDTO, Buyer>(buyer);
            await _buyersDAO.UpdateBuyer(buyerUpdateDTO, buyerId);
        }
        catch(ArgumentException){
            throw new Exception("Неверно указан пол.");
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }  
}
