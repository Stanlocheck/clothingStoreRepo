using AutoMapper;
using ClothesInterfacesBLL;
using ClothesInterfacesDAL;
using ClothDTOs;
using ClothDomain;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ClothingStoreApplication;

public class CartBusiness : ICartBLL
{
    private readonly ICartDAO _cartDAO;
    private Mapper _cartDTO;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartBusiness(ICartDAO cartDAO, IHttpContextAccessor httpContextAccessor){
        _cartDAO = cartDAO;
        _httpContextAccessor = httpContextAccessor;

        var _cartDtoMapping = new MapperConfiguration(cfg => {
            cfg.CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.Cloth, opt => opt.MapFrom(src => src.Cloth))
                .ForMember(dest => dest.Cart, opt => opt.MapFrom(src => src.Cart));
            cfg.CreateMap<Cart, CartDTO>()
                .ForMember(dest => dest.Buyer, opt => opt.MapFrom(src => src.Buyer))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            cfg.CreateMap<Buyer, BuyerDTO>();
            cfg.CreateMap<Cloth, ClothDTO>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));
        });
        _cartDTO = new Mapper(_cartDtoMapping);
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

    public async Task<CartDTO> GetCart(){
        try{
            var buyerId = GetLoggedInBuyerId();
            var cart = await _cartDAO.GetCart(buyerId);
            return _cartDTO.Map<Cart, CartDTO>(cart);
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

    public async Task AddAmountOfCartItem(Guid cartItemId){
        try{
            var buyerId = GetLoggedInBuyerId();
            await _cartDAO.AddAmountOfCartItem(buyerId, cartItemId);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task ReduceAmountOfCartItem(Guid cartItemId){
        try{
            var buyerId = GetLoggedInBuyerId();
            await _cartDAO.ReduceAmountOfCartItem(buyerId, cartItemId);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task DeleteCartItem(Guid cartItemId){
        try{
            var buyerId = GetLoggedInBuyerId();
            await _cartDAO.DeleteCartItem(buyerId, cartItemId);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task SelectCartItem(Guid cartItemId){
        try{
            var buyerId = GetLoggedInBuyerId();
            await _cartDAO.SelectCartItem(buyerId, cartItemId);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task<CartItemDTO> GetCartItem(Guid cartItemId){
        try{
            var buyerId = GetLoggedInBuyerId();
            var cartItem = await _cartDAO.GetCartItem(buyerId, cartItemId);
            return _cartDTO.Map<CartItem, CartItemDTO>(cartItem);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }
}
