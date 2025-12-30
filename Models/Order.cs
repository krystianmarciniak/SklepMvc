using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SklepMvc.Models;

public class Order
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = default!;
    public IdentityUser User { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    public decimal TotalPrice =>
        Items.Sum(i => i.UnitPrice * i.Quantity);
}
