using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SklepMvc.Data;
using SklepMvc.Models;
using SklepMvc.Helpers;

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

  public async Task<IActionResult> Index()
  {
    return await My();
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
      CreatedAt = DateTime.UtcNow,
      Items = new List<OrderItem>()
    };

    var item = new OrderItem
    {
      ProductId = product.Id,
      Quantity = quantity,
      UnitPrice = product.Price
    };

    order.Items.Add(item);

    product.Stock -= quantity;
    _db.Products.Update(product);

    _db.Orders.Add(order);
    await _db.SaveChangesAsync();

    await tx.CommitAsync();

    return RedirectToAction(nameof(My));
  }
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Checkout()
  {
    var cart = HttpContext.Session.GetObject<List<CartItem>>("CART");
    if (cart == null || !cart.Any())
      return RedirectToAction("Index", "Cart");

    var userId = _userManager.GetUserId(User)!;

    await using var tx = await _db.Database.BeginTransactionAsync();

    var order = new Order
    {
      UserId = userId,
      CreatedAt = DateTime.UtcNow,
      Items = new List<OrderItem>()
    };

    foreach (var c in cart)
    {
      var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == c.ProductId);
      if (product == null)
        continue;

      if (product.Stock < c.Quantity)
        continue;

      product.Stock -= c.Quantity;

      order.Items.Add(new OrderItem
      {
        ProductId = product.Id,
        Quantity = c.Quantity,
        UnitPrice = c.UnitPrice
      });
    }

    if (!order.Items.Any())
      return RedirectToAction("Index", "Cart");

    _db.Orders.Add(order);
    await _db.SaveChangesAsync();

    await tx.CommitAsync();

    HttpContext.Session.Remove("CART");

    return RedirectToAction("My");
  }
}
