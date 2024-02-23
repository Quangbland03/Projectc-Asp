using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using WebApplication10.Data;
using WebApplication10.Models;

namespace WebApplication10.Controllers
{
    public class HomeController : Controller
    {
		private readonly WebApplication10Context _context;

		public HomeController(WebApplication10Context context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var webApplication10Context = _context.Product.Include(p => p.Category);
			return View(await webApplication10Context.ToListAsync());
		}
        public async Task<IActionResult> Index1()
        {
            var webApplication10Context = _context.Product.Include(p => p.Category);
            return View(await webApplication10Context.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

		[HttpPost]
        public async Task<IActionResult> CreateCard(CartModel model)
        {
            var cart = HttpContext.Session.GetString("cart");

            if (cart == null)
            {
                var product = _context.Product.Find(model.Id);

                if (product != null)
                {
                    List<Cart> listCart = new List<Cart>()
            {
                new Cart
                {
                    Product = product,
                    Quantity = model.Quantity
                }
            };

                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(listCart));
                }
            }
            else
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);

                if (dataCart == null)
                {
                    // If the cart is null, create a new list and add the item
                    dataCart = new List<Cart>
            {
                new Cart
                {
                    Product = _context.Product.Find(model.Id),
                    Quantity = model.Quantity
                }
            };
                }
                else
                {
                    bool check = true;

                    for (int i = 0; i < dataCart.Count; i++)
                    {
                        if (dataCart[i].Product != null && dataCart[i].Product.Id == model.Id)
                        {
                            dataCart[i].Quantity++;
                            check = false;
                        }
                    }

                    if (check)
                    {
                        var product = _context.Product.Find(model.Id);

                        if (product != null)
                        {
                            dataCart.Add(new Cart
                            {
                                Product = product,
                                Quantity = model.Quantity
                            });
                        }
                    }
                }

                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
            }

            return RedirectToAction("ListCart");
        }


        public IActionResult ListCart()
        {

            var cart = HttpContext.Session.GetString("cart");
            if (cart != null)
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);

                if (dataCart.Count > 0)
                {
                    ViewBag.carts = dataCart;
                    return View();
                }
                return RedirectToAction(nameof(ListCart));
            }
            return RedirectToAction(nameof(ListCart));
        }
		
		public async Task<IActionResult> StoreOrder()
        {
            var cart = HttpContext.Session.GetString("cart");

            if (cart != null)
            {
                List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);

                foreach (var cartItem in dataCart)
                {
                    Order newOrder = new Order
                    {
                        OrderDate = DateTime.Now,
                        TotalPrice = cartItem.Product.Price * cartItem.Quantity,
                        ProductId = cartItem.Product.Id
                    };

                    _context.Order.Add(newOrder); // Thay _context.Add(Order) bằng _context.Orders.Add(newOrder)
                }

                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("cart");

                return RedirectToAction(nameof(ShowOrder));
            }

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ShowOrder()
        {
            var ordersWithProducts = await _context.Order
                .Join(
                    _context.Product,
                    order => order.ProductId,
                    product => product.Id,
                    (order, product) => new
                    {
                        Order = order,
                        Product = product
                    }
                )
                .ToListAsync();

            ViewBag.OrdersWithProducts = ordersWithProducts;

            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
