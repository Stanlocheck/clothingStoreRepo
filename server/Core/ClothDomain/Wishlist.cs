using System;

namespace ClothDomain;

public class Wishlist
{
    public Guid Id { get; set; }
    public Guid BuyerId { get; set; }
    public Buyer Buyer { get; set; }
    public ICollection<WishlistItem> Items { get; set; } = new List<WishlistItem>();
}
