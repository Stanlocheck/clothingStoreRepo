using System;

namespace ClothDTOs;

public class CartItemDTO
{
    public Guid Id { get; set; }
    public Guid ClothId { get; set; }
    public ClothDTO Cloth { get; set; }
    public int Amount { get; set; }
    public Guid CartId { get; set; }
    public CartDTO Cart { get; set; }
}
