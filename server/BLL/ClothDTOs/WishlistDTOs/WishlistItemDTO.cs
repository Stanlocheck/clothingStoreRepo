using System.Text.Json.Serialization;

namespace ClothDTOs.WishlistDTOs;

public class WishlistItemDTO
{
    public Guid Id { get; set; }
    public Guid ClothId { get; set; }

    [JsonIgnore]
    public ClothDTO Cloth { get; set; }

    [JsonIgnore]
    public Guid WishlistId { get; set; }
    
    [JsonIgnore]
    public WishlistDTO Wishlist { get; set; }
    public int Price { get; set; }
}
