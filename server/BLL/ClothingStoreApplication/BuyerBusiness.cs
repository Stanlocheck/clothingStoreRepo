using System;
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
    private Mapper _buyerAddDTO;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BuyerBusiness(IBuyersDAO buyersDAO, IHttpContextAccessor httpContextAccessor){
        _buyersDAO = buyersDAO;
        _httpContextAccessor = httpContextAccessor;

        var _buyerDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Buyer, BuyerDTO>().ReverseMap());
        _buyerDTO = new Mapper(_buyerDtoMapping);

        var _buyerAddDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Buyer, BuyerAddDTO>().ReverseMap());
        _buyerAddDTO = new Mapper(_buyerAddDtoMapping);
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

    public async Task UpdateBuyer(BuyerAddDTO buyerInfo){
        try{
            var buyer = new BuyerAddDTO {
                FirstName = buyerInfo.FirstName,
                LastName = buyerInfo.LastName,
                Email = buyerInfo.Email,
                Password = buyerInfo.Password,
                DateOfBirth = buyerInfo.DateOfBirth,
                Sex = buyerInfo.Sex.ToUpper(),
                PhoneNumber = buyerInfo.PhoneNumber,
                City = buyerInfo.City,
                StreetAddress = buyerInfo.StreetAddress,
                ApartmentNumber = buyerInfo.ApartmentNumber
            };
            var buyerId = GetLoggedInBuyerId();

            Enum.Parse<Gender>(buyer.Sex);
            var buyerUpdateDTO = _buyerAddDTO.Map<BuyerAddDTO, Buyer>(buyer);
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
