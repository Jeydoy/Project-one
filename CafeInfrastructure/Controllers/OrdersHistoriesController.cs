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
    public class OrdersHistoriesController : Controller
    {
        private readonly DbCafeContext _context;

        public OrdersHistoriesController(DbCafeContext context)
        {
            _context = context;
        }

        // GET: OrdersHistories
        public async Task<IActionResult> Index()
        {
            var dbCafeContext = _context.OrdersHistories.Include(o => o.Order).OrderByDescending(h=>h.ChangedAt);
            return View(await dbCafeContext.ToListAsync());
        }

        // GET: OrdersHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordersHistory = await _context.OrdersHistories
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordersHistory == null)
            {
                return NotFound();
            }

            return View(ordersHistory);
        }

        public async Task<IActionResult> HistoryByOrder(int? orderId)
        {
            if (orderId == null)
            {
                return NotFound();
            }
            var ordersHistories = await _context.OrdersHistories
                .Include(o => o.Order)
                .Where(h => h.OrderId == orderId)
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();
            if (ordersHistories == null || !ordersHistories.Any())
            {
                return NotFound();
            }
            ViewBag.OrderId = orderId;
            return View(ordersHistories);
        }

        // GET: OrdersHistories/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Payment");
            return View();
        }

        // POST: OrdersHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,ChangedAt,OldStatusId,NewStatusId,Id")] OrdersHistory ordersHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ordersHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Payment", ordersHistory.OrderId);
            return View(ordersHistory);
        }

        // GET: OrdersHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordersHistory = await _context.OrdersHistories.FindAsync(id);
            if (ordersHistory == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Payment", ordersHistory.OrderId);
            return View(ordersHistory);
        }

        // POST: OrdersHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,ChangedAt,OldStatusId,NewStatusId,Id")] OrdersHistory ordersHistory)
        {
            if (id != ordersHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordersHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdersHistoryExists(ordersHistory.Id))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Payment", ordersHistory.OrderId);
            return View(ordersHistory);
        }

        // GET: OrdersHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordersHistory = await _context.OrdersHistories
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordersHistory == null)
            {
                return NotFound();
            }

            return View(ordersHistory);
        }

        // POST: OrdersHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ordersHistory = await _context.OrdersHistories.FindAsync(id);
            if (ordersHistory != null)
            {
                _context.OrdersHistories.Remove(ordersHistory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdersHistoryExists(int id)
        {
            return _context.OrdersHistories.Any(e => e.Id == id);
        }
    }
}
