using AutoMapper;
using ClothesInterfacesBLL;
using ClothesInterfacesDAL;
using ClothDomain;
using ClothDTOs;
using ClothDTOs.WishlistDTOs;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ClothDTOs.ClothesDTOs;

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
            cfg.CreateMap<Cloth, ClothDTO>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));
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

    public async Task<List<WishlistItemDTO>> GetAllWishlistItems(){
        try{
            var buyerId = GetLoggedInBuyerId();
            var wishlist = await _wishlistDAO.GetAllWishlistItems(buyerId);
            return _wishlistItemDTO.Map<List<WishlistItemDTO>>(wishlist);
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

    public async Task FromWishlistToCart(Guid wishlistItemId){
        try{
            var buyerId = GetLoggedInBuyerId();
            await _wishlistDAO.FromWishlistToCart(buyerId, wishlistItemId);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task DeleteWishlistItem(Guid wishlistItemId){
        try{
            var buyerId = GetLoggedInBuyerId();
            await _wishlistDAO.FromWishlistToCart(buyerId, wishlistItemId);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }
}
