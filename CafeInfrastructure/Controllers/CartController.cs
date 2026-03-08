using Microsoft.AspNetCore.Mvc;
using CafeDomain.Model;
using CafeInfrastructure.Extensions;

namespace CafeInfrastructure.Controllers
{
    public class CartController : Controller
    {
        private readonly DbCafeContext _context;
        public CartController(DbCafeContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }
            var cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem == null)
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Quantity = 1,
                    Price = product.Price,
                    ProductName = product.Description
                });
            }
            else
            {
                cartItem.Quantity++;
            }
            HttpContext.Session.SetJson("Cart", cart);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
    }
}
