using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SklepMvc.Data;
using SklepMvc.Models;

namespace SklepMvc.Controllers;

[Authorize(Roles = "Admin")]
public class ProductsController : Controller
{
  private readonly ApplicationDbContext _db;

  public ProductsController(ApplicationDbContext db)
  {
    _db = db;
  }

  // GET: /Products
  public async Task<IActionResult> Index(string? q, decimal? minPrice, decimal? maxPrice, string? sort, string? dir)
  {
    var query = _db.Products.AsQueryable();

    if (!string.IsNullOrWhiteSpace(q))
    {
      var term = q.Trim().ToLower();
      query = query.Where(p =>
          p.Name.ToLower().Contains(term) ||
          (p.Description != null && p.Description.ToLower().Contains(term)));
    }

    if (minPrice.HasValue) query = query.Where(p => p.Price >= minPrice.Value);
    if (maxPrice.HasValue) query = query.Where(p => p.Price <= maxPrice.Value);

    var desc = string.Equals(dir, "desc", StringComparison.OrdinalIgnoreCase);

    query = (sort?.ToLower()) switch
    {
      "price" => desc ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
      "stock" => desc ? query.OrderByDescending(p => p.Stock) : query.OrderBy(p => p.Stock),
      _ => desc ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
    };

    ViewBag.Q = q;
    ViewBag.MinPrice = minPrice;
    ViewBag.MaxPrice = maxPrice;
    ViewBag.Sort = sort;
    ViewBag.Dir = dir;

    return View(await query.ToListAsync());
  }


  // GET: /Products/Details/5
  public async Task<IActionResult> Details(int? id)
  {
    if (id == null) return NotFound();

    var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
    if (product == null) return NotFound();

    return View(product);
  }

  // GET: /Products/Create
  public IActionResult Create()
  {
    return View();
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(Product product)
  {
    if (!ModelState.IsValid)
      return View(product);

    var name = product.Name?.Trim();

    if (string.IsNullOrWhiteSpace(name))
    {
      ModelState.AddModelError(nameof(Product.Name), "Nazwa jest wymagana.");
      return View(product);
    }

    var exists = await _db.Products.AnyAsync(p => p.Name.ToLower() == name.ToLower());
    if (exists)
    {
      ModelState.AddModelError(nameof(Product.Name), "Produkt o takiej nazwie już istnieje.");
      return View(product);
    }

    product.Name = name;

    _db.Products.Add(product);
    await _db.SaveChangesAsync();

    return RedirectToAction(nameof(Index));
  }


  // GET: /Products/Edit/5
  public async Task<IActionResult> Edit(int? id)
  {
    if (id == null) return NotFound();

    var product = await _db.Products.FindAsync(id);
    if (product == null) return NotFound();

    return View(product);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(int id, Product product)
  {
    if (id != product.Id)
      return NotFound();

    if (!ModelState.IsValid)
      return View(product);

    var name = product.Name?.Trim();
    if (string.IsNullOrWhiteSpace(name))
    {
      ModelState.AddModelError(nameof(Product.Name), "Nazwa jest wymagana.");
      return View(product);
    }

    var exists = await _db.Products.AnyAsync(p =>
        p.Id != product.Id && p.Name.ToLower() == name.ToLower());

    if (exists)
    {
      ModelState.AddModelError(nameof(Product.Name), "Produkt o takiej nazwie już istnieje.");
      return View(product);
    }

    try
    {
      product.Name = name;
      _db.Update(product);
      await _db.SaveChangesAsync();
    }
    catch
    {
      throw;
    }

    return RedirectToAction(nameof(Index));
  }


  // GET: /Products/Delete/5
  public async Task<IActionResult> Delete(int? id)
  {
    if (id == null) return NotFound();

    var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
    if (product == null) return NotFound();

    return View(product);
  }

  // POST: /Products/Delete/5
  [HttpPost, ActionName("Delete")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> DeleteConfirmed(int id)
  {
    var product = await _db.Products.FindAsync(id);
    if (product != null)
    {
      _db.Products.Remove(product);
      await _db.SaveChangesAsync();
    }

    return RedirectToAction(nameof(Index));
  }
}
