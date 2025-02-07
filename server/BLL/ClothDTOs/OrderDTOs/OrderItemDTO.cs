using System;
using System.Text.Json.Serialization;

namespace ClothDTOs.OrderDTOs;

public class OrderItemDTO
{
    public Guid Id { get; set; }
    public Guid ClothId { get; set; }
    [JsonIgnore]
    public ClothDTO Cloth { get; set; }
    public int Amount { get; set; }
    [JsonIgnore]
    public Guid OrderId { get; set; }
    [JsonIgnore]
    public OrderDTO Order { get; set; }
    public int Price { get; set; }
}
