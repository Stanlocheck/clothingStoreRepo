using ClothDTOs.ClothesDTOs;

namespace ClothDTOs;

public class ClothDTO
{
    public Guid Id { get; set; }
    public int Price { get; set; }
    public string Type { get; set; }
    public string Brand { get; set; }
    public string Season { get; set; }
    public int Size { get; set; }
    public string Material { get; set; }
    public string CountryOfOrigin { get; set; }
    public string Sex { get; set; }
    public int Count { get; set; }
    public ICollection<ClothImageDTO> Images { get; set; } = new List<ClothImageDTO>();
}
