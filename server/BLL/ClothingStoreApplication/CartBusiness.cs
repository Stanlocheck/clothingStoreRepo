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
    private Mapper _cartItemDTO;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartBusiness(ICartDAO cartDAO, IHttpContextAccessor httpContextAccessor){
        _cartDAO = cartDAO;
        _httpContextAccessor = httpContextAccessor;

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
}
