namespace ClothDTOs;

public class ClothDTO
{
    public Guid Id { get; set; }
    public int Price { get; set; }
    public string? Type { get; set; } = null;
    public string? Brand { get; set; } = null;
    public string? Season { get; set; } = null;
    public int? Size { get; set; } = null;
    public string? Material { get; set; } = null;
    public string? Manufacturer { get; set; } = null;
    public string Sex { get; set; }
}
