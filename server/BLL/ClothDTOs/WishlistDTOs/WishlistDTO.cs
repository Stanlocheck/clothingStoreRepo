using System;

namespace ClothDTOs.WishlistDTOs;

public class WishlistDTO
{
    public Guid Id { get; set; }
    public Guid BuyerId { get; set; }
    public BuyerDTO Buyer { get; set; }
    public ICollection<WishlistItemDTO> Items { get; set; } = new List<WishlistItemDTO>();
}
