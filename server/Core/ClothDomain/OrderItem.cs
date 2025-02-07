namespace ClothDomain;

public class OrderItem
{
    public Guid Id { get; set; }
    public Guid ClothId { get; set; }
    public Cloth Cloth { get; set; }
    public int Amount { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
    public int Price { get; set; }
}
