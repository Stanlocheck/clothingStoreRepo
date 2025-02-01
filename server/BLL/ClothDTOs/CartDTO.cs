using System;
using ClothDomain;

namespace ClothDTOs;

public class CartDTO
{
    public Guid Id { get; set; }
    public Guid BuyerId { get; set; }
    public BuyerDTO? Buyer { get; set; }
    public ICollection<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
}
