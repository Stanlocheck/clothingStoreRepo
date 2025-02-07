using AutoMapper;
using ClothesInterfacesBLL;
using ClothesInterfacesDAL;
using Microsoft.AspNetCore.Http;
using ClothDomain;
using ClothDTOs.OrderDTOs;
using ClothDTOs;
using System.Security.Claims;

namespace ClothingStoreApplication;

public class OrderBusiness : IOrderBLL
{
    private readonly IOrderDAO _orderDAO;
    private Mapper _orderDTO;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OrderBusiness(IOrderDAO orderDAO, IHttpContextAccessor httpContextAccessor){
        _orderDAO = orderDAO;
        _httpContextAccessor = httpContextAccessor;

        var _orderDtoMapping = new MapperConfiguration(cfg => {
            cfg.CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.Cloth, opt => opt.MapFrom(src => src.Cloth))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order));
            cfg.CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.Buyer, opt => opt.MapFrom(src => src.Buyer))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            cfg.CreateMap<Buyer, BuyerDTO>();
            cfg.CreateMap<Cloth, ClothDTO>();
        });
        _orderDTO = new Mapper(_orderDtoMapping);
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

    public async Task<List<OrderDTO>> GetAllOrders(){
        try{
            var buyerId = GetLoggedInBuyerId();
            var order = await _orderDAO.GetAllOrders(buyerId);
            return _orderDTO.Map<List<Order>, List<OrderDTO>>(order);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task<OrderDTO> GetOrder(Guid orderId){
        try{
            var buyerId = GetLoggedInBuyerId();
            var order = await _orderDAO.GetOrder(buyerId, orderId);
            return _orderDTO.Map<Order, OrderDTO>(order);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }

    public async Task CreateOrder(){
        try{
            var buyerId = GetLoggedInBuyerId();
            await _orderDAO.CreateOrder(buyerId);
        }
        catch(Exception ex){
            throw new Exception(ex.Message);
        }
    }
}
