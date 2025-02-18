using System.Text.Json.Serialization;

namespace ClothDTOs.OrderDTOs;

public class OrderDTO
{
    public Guid Id { get; set; }

    [JsonIgnore]
    public Guid BuyerId { get; set; }
    
    [JsonIgnore]
    public BuyerDTO Buyer { get; set; }
    public DateTime OrderDate { get; set; }
    public int Price { get; set; }
    public int Amount { get; set; }
    public string Status { get; set; }
    public ICollection<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();
}
