using ClothDTOs;
using ClothesInterfacesBLL;
using ClothesInterfacesDAL;
using ClothingStorePersistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using AutoMapper;
using ClothDomain;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;

namespace ClothingStoreApplication;

public class AuthService : IAuthService
{
    private readonly IBuyersDAO _buyersDAO;
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Mapper _buyerDTO;
    private Mapper _buyerAddDTO;

    public AuthService(IBuyersDAO buyersDAO, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor){
        _buyersDAO = buyersDAO;
        _context = context;
        _httpContextAccessor = httpContextAccessor;

        var _buyerDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Buyer, BuyerDTO>().ReverseMap());
        _buyerDTO = new Mapper(_buyerDtoMapping);

        var _buyerAddDtoMapping = new MapperConfiguration(cfg => cfg.CreateMap<Buyer, BuyerAddDTO>().ReverseMap());
        _buyerAddDTO = new Mapper(_buyerAddDtoMapping);
    }

    public async Task Register(BuyerAddDTO buyerInfo){
        if(await _context.Buyers.AnyAsync(b => b.Email == buyerInfo.Email)){
            throw new Exception("Пользователь с таким Email уже существует.");
        }

        if(!IsEmailValid(buyerInfo.Email)){
            throw new Exception("Неверный формат Email.");
        }

        if(!IsPasswordValid(buyerInfo.Password)){
            throw new Exception("Неверный формат пароля. Пароль должен содержать минимум 8 символов, 1 заглавную букву и 1 цифру.");
        }

        if(!IsPhoneNumberValid(buyerInfo.PhoneNumber)){
            throw new Exception("Неверный формат номера телефона.");
        }

        try{
            var buyer = new BuyerDTO {
                Id = Guid.NewGuid(),
                FirstName = buyerInfo.FirstName,
                LastName = buyerInfo.LastName,
                Email = buyerInfo.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(buyerInfo.Password),
                DateOfReg = DateTime.UtcNow,
                DateOfBirth = buyerInfo.DateOfBirth,
                Sex = buyerInfo.Sex.ToUpper(),
                PhoneNumber = buyerInfo.PhoneNumber,
                City = buyerInfo.City,
                StreetAddress = buyerInfo.StreetAddress,
                ApartmentNumber = buyerInfo.ApartmentNumber,
                Role = "Buyer"
            };
            Enum.Parse<Gender>(buyer.Sex);
            var buyerAddDTO = _buyerDTO.Map<BuyerDTO, Buyer>(buyer);
            await _buyersDAO.AddBuyer(buyerAddDTO);
        }
        catch(ArgumentException){
            throw new Exception("Неверно указан пол.");
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }

    }

    public async Task Login(string email, string password){
        var buyer = await _context.Buyers.FirstOrDefaultAsync(b => b.Email == email);
        if (buyer == null || !BCrypt.Net.BCrypt.Verify(password, buyer.Password))
        {
            throw new Exception("Неверный Email или пароль.");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, buyer.Email),
            new Claim(ClaimTypes.NameIdentifier, buyer.Id.ToString()),
            new Claim(ClaimTypes.Role, buyer.Role)
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties();

        await _httpContextAccessor.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }

    public async Task Logout(){
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    private bool IsPasswordValid(string password){
        var regex = new Regex(@"^(?=.*[A-Z])(?=.*\d).{8,}$");
        return regex.IsMatch(password);
    }
    private bool IsEmailValid(string email){
        var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        return regex.IsMatch(email);
    }
    private bool IsPhoneNumberValid(string phoneNumber){
        var regex = new Regex(@"^(?:\+7|8)?[\s\-]?\(?\d{3}\)?[\s\-]?\d{3}[\s\-]?\d{2}[\s\-]?\d{2}$");
        return regex.IsMatch(phoneNumber);
    }
}