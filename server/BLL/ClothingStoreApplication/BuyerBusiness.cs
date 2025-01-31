using System;
using AutoMapper;
using ClothDomain;
using ClothDTOs;
using ClothesInterfacesBLL;
using ClothesInterfacesDAL;

namespace ClothingStoreApplication;

public class BuyerBusiness : IBuyersBLL
{
    private readonly IBuyersDAO _buyersDAO;
    private Mapper _buyerDTO;
    private Mapper _buyerAddDTO;

    public BuyerBusiness(IBuyersDAO buyersDAO){
        _buyersDAO = buyersDAO;

        var _buyerDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Buyer, BuyerDTO>().ReverseMap());
        _buyerDTO = new Mapper(_buyerDtoMapping);

        var _buyerAddDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Buyer, BuyerAddDTO>().ReverseMap());
        _buyerAddDTO = new Mapper(_buyerAddDtoMapping);
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

     public async Task AddBuyer(BuyerAddDTO buyerInfo){
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
            Enum.Parse<Gender>(buyer.Sex);
            var buyerAddDTO = _buyerAddDTO.Map<BuyerAddDTO, Buyer>(buyer);
            await _buyersDAO.AddBuyer(buyerAddDTO);
        }
        catch(ArgumentException){
            throw new Exception("Неверно указан пол.");
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
     }

     public async Task UpdateBuyer(BuyerAddDTO buyerInfo, Guid id){
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
            Enum.Parse<Gender>(buyer.Sex);
            var buyerUpdateDTO = _buyerAddDTO.Map<BuyerAddDTO, Buyer>(buyer);
            await _buyersDAO.UpdateBuyer(buyerUpdateDTO, id);
        }
        catch(ArgumentException){
            throw new Exception("Неверно указан пол.");
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
     }

     public async Task DeleteBuyer(Guid id){
        try{
            await _buyersDAO.DeleteBuyer(id);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
     }    
}
