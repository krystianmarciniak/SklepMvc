using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SklepMvc.Data;
using SklepMvc.Helpers;
using SklepMvc.Models;

namespace SklepMvc.Controllers;

public class CartController : Controller
{
  private const string CartKey = "CART";
  private readonly ApplicationDbContext _db;

  public CartController(ApplicationDbContext db)
  {
    _db = db;
  }

  // GET: /Cart
  public IActionResult Index()
  {
    var cart = HttpContext.Session.GetObject<List<CartItem>>(CartKey) ?? new List<CartItem>();
    return View(cart);
  }

  // POST: /Cart/Add
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Add(int productId, int quantity = 1)
  {
    if (quantity < 1) quantity = 1;

    var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId);
    if (product == null) return NotFound();

    var cart = HttpContext.Session.GetObject<List<CartItem>>(CartKey) ?? new List<CartItem>();

    var existing = cart.FirstOrDefault(c => c.ProductId == productId);
    if (existing != null)
    {
      existing.Quantity += quantity;
    }
    else
    {
      cart.Add(new CartItem
      {
        ProductId = product.Id,
        Name = product.Name,
        UnitPrice = product.Price,
        Quantity = quantity
      });
    }

    HttpContext.Session.SetObject(CartKey, cart);
    return RedirectToAction("Index");
  }

  // POST: /Cart/Remove
  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult Remove(int productId)
  {
    var cart = HttpContext.Session.GetObject<List<CartItem>>(CartKey) ?? new List<CartItem>();
    cart.RemoveAll(c => c.ProductId == productId);

    HttpContext.Session.SetObject(CartKey, cart);
    return RedirectToAction("Index");
  }

  // POST: /Cart/Clear
  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult Clear()
  {
    HttpContext.Session.Remove(CartKey);
    return RedirectToAction("Index");
  }
}
