using System.Text.Json.Serialization;

namespace ClothDTOs;

public class CartItemDTO
{
    public Guid Id { get; set; }
    public Guid ClothId { get; set; }

    [JsonIgnore]
    public ClothDTO Cloth { get; set; }
    public int Amount { get; set; }
    public Guid CartId { get; set; }

    [JsonIgnore]
    public CartDTO Cart { get; set; }
    public bool Selected { get; set; }
    public int Price { get; set; }
}
