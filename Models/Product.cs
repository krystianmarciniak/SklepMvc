using System.ComponentModel.DataAnnotations;

namespace SklepMvc.Models;

public class Product
{
  public int Id { get; set; }

  [Required, StringLength(100)]
  public string Name { get; set; } = "";

  [StringLength(500)]
  public string? Description { get; set; }

  [Range(0.01, 100000)]
  public decimal Price { get; set; }

  [Range(0, 100000)]
  public int Stock { get; set; } = 0;

  public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
