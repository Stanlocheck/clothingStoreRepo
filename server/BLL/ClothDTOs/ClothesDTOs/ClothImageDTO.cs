using System;
using System.Text.Json.Serialization;

namespace ClothDTOs.ClothesDTOs;

public class ClothImageDTO
{
    public Guid Id { get; set; }

    [JsonIgnore]
    public byte[] Data { get; set; }
    public string ContentType { get; set; }

    [JsonIgnore]
    public Guid ClothId { get; set; }

    [JsonIgnore]
    public ClothDTO Cloth { get; set; }
}
