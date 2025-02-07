using System.ComponentModel.DataAnnotations.Schema;

namespace ClothDomain;

public class Order
{
    public Guid Id { get; set; }
    public Guid BuyerId { get; set; }
    public Buyer Buyer { get; set; }
    public DateTime OrderDate { get; set; }
    public int Price { get; set; }
    public int Amount { get; set; }
    [Column(TypeName = "text")]
    public OrderStatus Status { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
