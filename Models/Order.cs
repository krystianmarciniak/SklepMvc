using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SklepMvc.Models;

public class Order
{
    public int Id { get; set; }

    [Required, StringLength(40)]
    public string OrderNumber { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required, StringLength(30)]
    public string Status { get; set; } = "Nowe";

    [Required]
    public string UserId { get; set; } = string.Empty;
    public IdentityUser? User { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}
