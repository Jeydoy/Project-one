using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CafeDomain.Model;
using CafeInfrastructure;

namespace CafeInfrastructure.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DbCafeContext _context;

        public ProductsController(DbCafeContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(int? categoryID, decimal? minPrice, decimal? maxPrice, string searchString )
        {
           var products = _context.Products
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Description.Contains(searchString));
            }

            if ( categoryID.HasValue)
            {
                products = products.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryID));
            }

            if(minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue) {
                products = products.Where(p => p.Price <= maxPrice.Value); 
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "CategoryName"); 
            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new MultiSelectList(_context.Categories, "Id", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Photo,IsAvaliable,Price,Description,CreatedAt,UpdatedAt,Id")] Product product, int[] selectedCategories)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();

                if(selectedCategories!= null && selectedCategories.Length > 0)
                {
                    foreach (var categoryId in selectedCategories)
                    {
                        var productCategory = new ProductCategory
                        {
                            ProductId = product.Id,
                            CategoryId = categoryId
                        };
                        _context.ProductCategories.Add(productCategory);
                    }
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = new MultiSelectList(_context.Categories, "Id", "CategoryName");
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductCategories)
                    .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            var selectedCategoryIds = product.ProductCategories.Select(pc => pc.CategoryId).ToArray();
            ViewBag.Categories = new MultiSelectList(_context.Categories, "Id", "CategoryName", selectedCategoryIds);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Photo,IsAvaliable,Price,Description,CreatedAt,UpdatedAt,Id")] Product product, int[] selectedCategories)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    var existingLinks = _context.ProductCategories.Where(pc => pc.ProductId == product.Id);
                    _context.ProductCategories.RemoveRange(existingLinks);
                    
                    if (selectedCategories != null && selectedCategories.Length > 0)
                    {
                        foreach (var categoryId in selectedCategories)
                        {
                            var productCategory = new ProductCategory
                            {
                                ProductId = product.Id,
                                CategoryId = categoryId
                            };
                            _context.ProductCategories.Add(productCategory);
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
