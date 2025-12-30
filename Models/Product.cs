using System.ComponentModel.DataAnnotations;

namespace SklepMvc.Models;

public class Product
{
  public int Id { get; set; }

  [Required]
  [StringLength(80, MinimumLength = 2)]
  public string Name { get; set; } = string.Empty;

  [StringLength(400)]
  public string? Description { get; set; }

  [Range(0.01, 1_000_000)]
  [DataType(DataType.Currency)]
  public decimal Price { get; set; }

  [Range(0, 100_000)]
  public int Stock { get; set; }
}
