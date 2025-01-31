using System;

namespace ClothDTOs;

public class CartItemAddDTO
{
    //public Guid ClothId { get; set; }
    public ClothDTO Cloth { get; set; }
    public int Amount { get; set; }
    public Guid CartId { get; set; }
    public CartDTO Cart { get; set; }    
}
