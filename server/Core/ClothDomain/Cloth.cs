using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ClothDomain;

public class Cloth
{
    public Guid Id { get; set; }
    public int Price { get; set; }
    public string Type { get; set; }
    public string Brand { get; set; }
    public string Season { get; set; }
    public int Size { get; set; }
    public string Material { get; set; }
    public string CountryOfOrigin { get; set; }
    [Column(TypeName = "text")]
    public Gender Sex { get; set; }
    public ICollection<ClothImage> Images { get; set; } = new List<ClothImage>();
}
