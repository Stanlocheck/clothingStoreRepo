using System.Text.Json.Serialization;

namespace ClothDTOs;

public class CartDTO
{
    public Guid Id { get; set; }
    [JsonIgnore]
    public Guid BuyerId { get; set; }
    [JsonIgnore]
    public BuyerDTO Buyer { get; set; }
    public int Price { get; set; }
    public ICollection<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
}
