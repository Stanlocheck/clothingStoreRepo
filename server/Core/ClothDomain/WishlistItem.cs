namespace ClothDomain;

public class WishlistItem
{
    public Guid Id { get; set; }
    public Guid ClothId { get; set; }
    public Cloth Cloth { get; set; }
    public Guid WishlistId { get; set; }
    public Wishlist Wishlist { get; set; }
    public int Price { get; set; }
}
