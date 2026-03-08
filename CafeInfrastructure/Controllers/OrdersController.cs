using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CafeDomain.Model;
using CafeInfrastructure;
using CafeInfrastructure.Extensions;

namespace CafeInfrastructure.Controllers
{
    public class OrdersController : Controller
    {
        private readonly DbCafeContext _context;

        public OrdersController(DbCafeContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var dbCafeContext = _context.Orders.Include(o => o.Status).Include(o => o.User).OrderByDescending(o=> o.CreatedAt);
            return View(await dbCafeContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Status)
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                   .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public IActionResult Checkout()
        {
                       return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout([Bind("Adress, Payment")] Order order)
        {
            var cart= HttpContext.Session.GetJson<List<CartItem>>("Cart");
            order.CreatedAt = DateTime.Now;
            order.UpdatedAt = DateTime.Now;
            order.UserId = 1;
            order.StatusId = 1;
            if(ModelState.IsValid)
            {
               using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();
                    
                    foreach (var cartItem in cart)
                    {
                        _context.OrderItems.Add(new OrderItem
                        {
                            OrderId = order.Id,
                            ProductId = cartItem.ProductId,
                            Amount = cartItem.Quantity,
                            PriceForNow = cartItem.Price
                        });
                     
                    }
                    await _context.SaveChangesAsync();
                    HttpContext.Session.Remove("Cart");
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    ModelState.AddModelError("", "Виникла помилка при обробці вашого замовлення, спробуйте знову");
                    return View(order);
                }
                 return RedirectToAction(nameof(Index));
            }

            return View(order);

        }

        public IActionResult Payment(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["StatusId"] = new SelectList(_context.OrdersStatuses, "Id", "Status");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StatusId,UserId,Price,Adress,Payment,UpdatedAt,CreatedAt,Id")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StatusId"] = new SelectList(_context.OrdersStatuses, "Id", "Status", order.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["StatusId"] = new SelectList(_context.OrdersStatuses, "Id", "Status", order.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StatusId,UserId,Price,Adress,Payment,UpdatedAt,CreatedAt,Id")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["StatusId"] = new SelectList(_context.OrdersStatuses, "Id", "Status", order.StatusId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Status)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
