namespace SklepMvc.Models;

public class CartItem
{
  public int ProductId { get; set; }
  public string Name { get; set; } = "";
  public decimal UnitPrice { get; set; }
  public int Quantity { get; set; }
}
