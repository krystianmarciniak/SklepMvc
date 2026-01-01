using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SklepMvc.Data;
using SklepMvc.Models;

namespace SklepMvc.Controllers;

[Authorize]
public class OrdersController : Controller
{
  private readonly ApplicationDbContext _db;
  private readonly UserManager<IdentityUser> _userManager;

  public OrdersController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
  {
    _db = db;
    _userManager = userManager;
  }
  [Authorize]
  public async Task<IActionResult> My()
  {
    var userId = _userManager.GetUserId(User)!;

    var orders = await _db.Orders
        .Where(o => o.UserId == userId)
        .OrderByDescending(o => o.CreatedAt)
        .Include(o => o.Items)
            .ThenInclude(i => i.Product)
        .ToListAsync();

    return View(orders);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> CreateQuick(int productId, int quantity)
  {
    if (quantity < 1)
    {
      ModelState.AddModelError(nameof(quantity), "Ilość musi być >= 1.");
      return RedirectToAction("Index", "Products");
    }

    var userId = _userManager.GetUserId(User)!;

    await using var tx = await _db.Database.BeginTransactionAsync();

    var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId);
    if (product == null)
      return NotFound();

    if (product.Stock < quantity)
    {
      ModelState.AddModelError("", "Brak wystarczającej ilości na stanie.");
      return RedirectToAction("Details", "Products", new { id = productId });
    }

    var order = new Order
    {
      UserId = userId,
      CreatedAt = DateTime.UtcNow
    };

    var item = new OrderItem
    {
      ProductId = product.Id,
      Quantity = quantity,
      UnitPrice = product.Price
    };

    order.Items.Add(item);

    product.Stock -= quantity;

    _db.Orders.Add(order);
    await _db.SaveChangesAsync();

    await tx.CommitAsync();

    return RedirectToAction(nameof(My));
  }
}
