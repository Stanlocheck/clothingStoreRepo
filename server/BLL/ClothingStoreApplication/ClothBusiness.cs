using System.Runtime.InteropServices;
using System.Security.Claims;
using AutoMapper;
using ClothDomain;
using ClothDTOs;
using ClothDTOs.WishlistDTOs;
using ClothesInterfacesBLL;
using ClothesInterfacesDAL;
using ClothingStorePersistence;
using Microsoft.AspNetCore.Http;

namespace ClothingStoreApplication;

public class ClothBusiness : IClothBLL, ICartBLL, IWishlistBLL
{
    private Mapper _clothDTO;
    private Mapper _clothAddDTO;
    private Mapper _cartItemDTO;
    private Mapper _wishlistItemDTO;
    private readonly IClothesDAO _clothDAO;
    private readonly ICartDAO _cartDAO;
    private readonly IWishlistDAO _wishlistDAO;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ClothBusiness(IClothesDAO clothDAO, ICartDAO cartDAO, IWishlistDAO wishlistDAO, IHttpContextAccessor httpContextAccessor){
        _clothDAO = clothDAO;
        _cartDAO = cartDAO;
        _wishlistDAO = wishlistDAO;
        _httpContextAccessor = httpContextAccessor;

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

        var _wishlistItemDtoMapping = new MapperConfiguration(cfg => {
            cfg.CreateMap<WishlistItem, WishlistItemDTO>()
                .ForMember(dest => dest.Cloth, opt => opt.MapFrom(src => src.Cloth))
                .ForMember(dest => dest.Wishlist, opt => opt.MapFrom(src => src.Wishlist));
            cfg.CreateMap<Wishlist, WishlistDTO>()
                .ForMember(dest => dest.Buyer, opt => opt.MapFrom(src => src.Buyer))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            cfg.CreateMap<Buyer, BuyerDTO>();
            cfg.CreateMap<Cloth, ClothDTO>();
        });
        _wishlistItemDTO = new Mapper(_wishlistItemDtoMapping);
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

    public async Task UpdateCloth(ClothAddDTO clothInfo, Guid id){
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

    public async Task AddToCart(Guid clothId){
        try{
            var buyerId = GetLoggedInBuyerId();
            await _cartDAO.AddToCart(buyerId, clothId);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<CartItemDTO>> GetCartItems(){
        try{
            var buyerId = GetLoggedInBuyerId();
            var cart = await _cartDAO.GetCartItems(buyerId);
            return _cartItemDTO.Map<List<CartItemDTO>>(cart);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task AddAmountOfCartItem(Guid cartId){
        try{
            var buyerId = GetLoggedInBuyerId();
            await _cartDAO.AddAmountOfCartItem(buyerId, cartId);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task ReduceAmountOfCartItem(Guid cartId){
        try{
            var buyerId = GetLoggedInBuyerId();
            await _cartDAO.ReduceAmountOfCartItem(buyerId, cartId);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task DeleteCartItem(Guid cartId){
        try{
            var buyerId = GetLoggedInBuyerId();
            await _cartDAO.DeleteCartItem(buyerId, cartId);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task AddToWishlist(Guid clothId){
        try{
            var buyerId = GetLoggedInBuyerId();
            await _wishlistDAO.AddToWishlist(buyerId, clothId);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<WishlistItemDTO>> GetWishlistItems(){
        try{
            var buyerId = GetLoggedInBuyerId();
            var wishlist = await _wishlistDAO.GetWishlistItems(buyerId);
            return _wishlistItemDTO.Map<List<WishlistItemDTO>>(wishlist);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task FromWishlistToCart(Guid wishlistId){
        try{
            var buyerId = GetLoggedInBuyerId();
            await _wishlistDAO.FromWishlistToCart(buyerId, wishlistId);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task DeleteWishlistItem(Guid wishlistId){
        try{
            var buyerId = GetLoggedInBuyerId();
            await _wishlistDAO.FromWishlistToCart(buyerId, wishlistId);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }
}
