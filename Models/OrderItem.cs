using System.ComponentModel.DataAnnotations;

namespace SklepMvc.Models;

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    [Range(1, 100_000)]
    public int Quantity { get; set; }

    [Range(0.01, 1_000_000)]
    public decimal UnitPrice { get; set; }
}
