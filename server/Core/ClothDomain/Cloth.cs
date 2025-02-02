using System.ComponentModel.DataAnnotations.Schema;

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
    public string Manufacturer { get; set; }
    [Column(TypeName = "text")]
    public Gender Sex { get; set; }
}
