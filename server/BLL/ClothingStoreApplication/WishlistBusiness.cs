using AutoMapper;
using ClothesInterfacesBLL;
using ClothesInterfacesDAL;
using ClothDomain;
using ClothDTOs;
using ClothDTOs.WishlistDTOs;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ClothingStoreApplication;

public class WishlistBusiness : IWishlistBLL
{
    private readonly IWishlistDAO _wishlistDAO;
    private Mapper _wishlistItemDTO;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WishlistBusiness(IWishlistDAO wishlistDAO, IHttpContextAccessor httpContextAccessor){
        _wishlistDAO = wishlistDAO;
        _httpContextAccessor = httpContextAccessor;

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
