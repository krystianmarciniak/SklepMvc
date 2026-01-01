using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SklepMvc.Models;

public class Order
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public string UserId { get; set; } = string.Empty;
    public IdentityUser? User { get; set; }

    public List<OrderItem> Items { get; set; } = new();
}
