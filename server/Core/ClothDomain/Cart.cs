namespace ClothDomain;

public class Cart
{
    public Guid Id { get; set; }
    public Guid BuyerId { get; set; }
    public Buyer Buyer { get; set; }
    public int Price { get; set; }
    public int Amount { get; set; }
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}
