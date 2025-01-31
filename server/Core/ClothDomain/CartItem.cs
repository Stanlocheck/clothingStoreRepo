using System;

namespace ClothDomain;

public class CartItem
{
    public Guid Id { get; set; }
    public Guid ClothId { get; set; }
    public Cloth? Cloth { get; set; }
    public int Amount { get; set; }
    public Guid CartId { get; set; }
    public Cart? Cart { get; set; }
}
