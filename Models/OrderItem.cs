using System.ComponentModel.DataAnnotations;

namespace SklepMvc.Models;

public class OrderItem
{
  public int Id { get; set; }

  [Required]
  public int OrderId { get; set; }
  public Order? Order { get; set; }

  [Required]
  public int ProductId { get; set; }
  public Product? Product { get; set; }

  [Range(1, 1000)]
  public int Quantity { get; set; }

  [Range(0.01, 100000)]
  public decimal UnitPrice { get; set; }
}
