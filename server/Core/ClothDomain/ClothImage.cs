using System;
using System.Text.Json.Serialization;

namespace ClothDomain;

public class ClothImage
{
    public Guid Id { get; set; }
    public byte[] Data { get; set; }
    public string ContentType { get; set; }
    public Guid ClothId { get; set; }
    public Cloth Cloth { get; set; }
}
